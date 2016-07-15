
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    /// <summary>
    /// All action defines
    /// </summary>
    public partial class Action : MonoBehaviour
    {
        public enum eActionType { Finite = 0, Interval }


        /// <summary>
        /// Action base
        /// </summary>
        public abstract class ActionBase
        {
            protected ActionBase(eActionType type, float duration = 0)
            {
                _type = type;
                this.duration = duration;
            }

            public virtual void Start(GameObject target)
            {
                this.target = target;
                elapsed = 0;
                percent = 0;
            }

            public virtual void Stop() { }

            public abstract float Step(float deltaTime);

            protected virtual void Update() { }

            public abstract bool IsDone();

            public float GetDuration() { return duration; }

            public IEnumerator WaitForDone()
            {
                while (!IsDone())
                {
                    yield return null;
                }
            }

            protected GameObject target = null;
            protected float duration = 0;
            protected float elapsed = 0.0f;
            protected float percent = 0.0f;
            protected float deltaTime = 0.0f;

            protected eActionType type { get { return _type; } }
            protected eActionType _type;
        }


        /// <summary>
        /// Finite action
        /// </summary>
        public abstract class FiniteAction : ActionBase
        {
            protected FiniteAction() : base(eActionType.Finite) { }

            public override float Step(float deltaTime)
            {
                if (IsDone()) { return deltaTime; }
                percent = 1.0f;
                Update();
                return deltaTime;
            }

            public override bool IsDone() { return (percent > 0.0f); }
        }


        /// <summary>
        /// Interval action
        /// </summary>
        public abstract class IntervalAction : ActionBase
        {
            protected IntervalAction(float duration) : base(eActionType.Interval, duration) { }

            public override float Step(float deltaTime)
            {
                if (IsDone()) { return deltaTime; }
                this.deltaTime = deltaTime;
                elapsed += deltaTime;
                if (elapsed > duration)
                {
                    this.deltaTime = elapsed - duration;
                    elapsed = duration;
                }
                percent = Mathf.Max(0, Mathf.Min(1.0f, elapsed / Mathf.Max(duration, Mathf.Epsilon)));
                Update();
                return deltaTime - this.deltaTime;
            }

            public override bool IsDone() { return (elapsed >= duration); }
        }


        /// <summary>
        /// CallFunc
        /// </summary>
        public class CallFunc : FiniteAction
        {
            public CallFunc(Callback callback)
            {
                this.callback = callback;
            }

            protected override void Update()
            {
                if (callback == null) { return; }
                callback();
            }

            protected Callback callback = null;

            public delegate void Callback();
        }


        /// <summary>
        /// Sequence
        /// </summary>
        public class Sequence : IntervalAction
        {
            public Sequence()
                : base(0.0f)
            {

            }

            public Sequence(List<ActionBase> list)
                : base(0.0f)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    actionList.Add(list[i]);
                }
            }

            public Sequence(params ActionBase[] actions)
                : base(0.0f)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    ActionBase action = actions[i];
                    actionList.Add(action);
                }
            }

            public static Sequence CreateWithActions(params ActionBase[] actions)
            {
                return new Sequence(actions);
            }

            public void Add(ActionBase action)
            {
                actionList.Add(action);
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                curActionIndex = 0;
                if (actionList.Count <= 0) return;
                actionList[0].Start(target);
            }

            public override float Step(float deltaTime)
            {
                if (IsDone()) { return deltaTime; }

                // Step
                float rest = deltaTime;
                ActionBase action = actionList[curActionIndex];
                rest = action.Step(rest);

                // Start next and continue step if previous done
                if (action.IsDone())
                {
                    action.Stop();
                    ++curActionIndex;
                    if (curActionIndex >= actionList.Count) { return rest; }
                    ActionBase nextAction = actionList[curActionIndex];
                    nextAction.Start(target);
                    return Step(rest);
                }

                return rest;
            }

            public override bool IsDone() { return (curActionIndex >= actionList.Count); }

            protected List<ActionBase> actionList = new List<ActionBase>();
            protected int curActionIndex = 0;
        }


        /// <summary>
        /// Spawn
        /// </summary>
        public class Spawn : IntervalAction
        {
            public Spawn()
                : base(0.0f)
            {

            }

            public Spawn(List<ActionBase> list)
                : base(0.0f)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    actionList.Add(list[i]);
                }
            }

            public Spawn(params ActionBase[] actions)
                : base(0.0f)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    ActionBase action = actions[i];
                    actionList.Add(action);
                }
            }

			public static Spawn CreateWithActions(params ActionBase[] actions)
			{
				return new Spawn(actions);
			}

            public void Add(ActionBase action)
            {
                actionList.Add(action);
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                for (int i = 0; i < actionList.Count; ++i)
                {
                    actionList[i].Start(target);
                }
                allDone = false;
            }

            public override float Step(float deltaTime)
            {
                if (IsDone()) { return deltaTime; }
                allDone = true;
                float minRest = deltaTime;
                for (int i = 0; i < actionList.Count; i++)
                {
                    ActionBase action = actionList[i];
                    if (action.IsDone()) { continue; }
                    float rest = action.Step(deltaTime);
                    minRest = rest < minRest ? rest : minRest;
                    if (action.IsDone())
                    {
                        action.Stop();
                    }
                    else
                    {
                        allDone = false;
                    }
                }
                return minRest;
            }

            public override bool IsDone() { return allDone; }

            protected List<ActionBase> actionList = new List<ActionBase>();
            protected bool allDone = false;
        }



        /// <summary>
        /// Delay
        /// </summary>
        public class Delay : IntervalAction
        {
            public Delay(float duration)
                : base(duration)
            {

            }
        }


        /// <summary>
        /// Repeat
        /// </summary>
        public class Repeat : IntervalAction
        {
            public Repeat(ActionBase action, int count = -1)
                : base(0)
            {
                this.action = action;
                this.count = count;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                curCount = 0;
            }

            public override float Step(float deltaTime)
            {
                if (IsDone()) { return deltaTime; }
                float rest = action.Step(deltaTime);
                if (action.IsDone())
                {
                    ++curCount;
                    if (!IsDone())
                    {
                        action.Start(target);
                    }
                }
                return rest;
            }

            public override bool IsDone() { return (count < 0) ? false : curCount >= count; }

            protected ActionBase action;
            protected int count = 0;
            protected int curCount = 0;
        }


        /// <summary>
        /// Move by
        /// </summary>
        public class MoveBy : IntervalAction
        {
            public MoveBy(float duration, Vector3 deltaPosition)
                : base(duration)
            {
                this.deltaPosition = deltaPosition;
                this.space = Space.World;
            }

            public MoveBy(float duration, Vector3 deltaPosition, Space space)
                : base(duration)
            {
                this.deltaPosition = deltaPosition;
                this.space = space;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                if (space == Space.World)
                {
                    startPosition = target.transform.position;
                }
                else
                {
                    startPosition = target.transform.localPosition;
                }
            }

            protected override void Update()
            {
                Vector3 position = startPosition + deltaPosition * percent;
                if (space == Space.World)
                {
                    target.transform.position = position;
                }
                else
                {
                    target.transform.localPosition = position;
                }
            }

            protected Vector3 deltaPosition;
            protected Space space;
            protected Vector3 startPosition;
        }


        /// <summary>
        /// Move To
        /// </summary>
        public class MoveTo : MoveBy
        {
            public MoveTo(float duration, Vector3 position, Space space)
                : base(duration, position, space)
            {

            }

			public MoveTo(float duration, Vector3 position)
				: base(duration, position, Space.World)
			{
				
			}

            public override void Start(GameObject target)
            {
                if (space == Space.World)
                {
                    deltaPosition -= target.transform.position;
                }
                else
                {
                    startPosition -= target.transform.localPosition;
                }
                base.Start(target);
            }
        }



        /// <summary>
        /// Rotate by
        /// </summary>
        public class RotateBy : IntervalAction
        {
            public RotateBy(float duration, Vector3 deltaEular, Space space = Space.World)
                : base(duration)
            {
                this.deltaEular = deltaEular;
                this.space = space;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                if (space == Space.World)
                {
                    startEular = target.transform.eulerAngles;
                }
                else
                {
                    startEular = target.transform.localEulerAngles;
                }
            }

            protected override void Update()
            {
                Vector3 eular = startEular + deltaEular * percent;
                if (space == Space.World)
                {
                    target.transform.eulerAngles = eular;
                }
                else
                {
                    target.transform.localEulerAngles = eular;
                }
            }

            protected Vector3 deltaEular;
            protected Space space;
            protected Vector3 startEular;
        }


        /// <summary>
        /// Rotate to
        /// </summary>
        public class RotateTo : RotateBy
        {
            public RotateTo(float duration, Vector3 eular, Space space = Space.World)
                : base(duration, eular, space)
            {

            }

            public override void Start(GameObject target)
            {
                if (space == Space.World)
                {
                    deltaEular -= target.transform.eulerAngles;
                }
                else
                {
                    deltaEular -= target.transform.localEulerAngles;
                }
                base.Start(target);
            }
        }


        /// <summary>
        /// Rotate Around
        /// </summary>
        public class RotateAround : IntervalAction
        {
            public RotateAround(float duration, Vector3 point, Vector3 axis, float angle)
                : base(duration)
            {
                this.point = point;
                this.axis = axis;
                this.angle = angle;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                sumAngle = 0;
            }

            protected override void Update()
            {
                float curAngle = angle * percent;
                float subAngle = Mathf.Max(0, angle - curAngle);
                sumAngle = curAngle;
                target.transform.RotateAround(point, axis, subAngle);
            }

            protected Vector3 point;
            protected Vector3 axis;
            protected float angle;
            protected float sumAngle;
        }


        /// <summary>
        /// Scale By
        /// </summary>
        public class ScaleBy : IntervalAction
        {
            public ScaleBy(float duration, Vector3 deltaScale)
                : base(duration)
            {
                this.deltaScale = deltaScale;
            }

            public ScaleBy(float duration, float s)
                : base(duration)
            {
                this.deltaScale = new Vector3(s, s, s);
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                startScale = target.transform.localScale;
            }

            protected override void Update()
            {
                Vector3 scale = startScale + deltaScale * percent;
                target.transform.localScale = scale;
            }

            protected Vector3 deltaScale;
            protected Vector3 startScale;
        }


        /// <summary>
        /// Scale To 
        /// </summary>
        public class ScaleTo : ScaleBy
        {
            public ScaleTo(float duration, Vector3 scale)
                : base(duration, scale)
            {
            }

            public ScaleTo(float duration, float s)
                : base(duration, s)
            {
            }

            public override void Start(GameObject target)
            {
                deltaScale -= target.transform.localScale;
                base.Start(target);
            }
        }


        /// <summary>
        /// Blink 
        /// </summary>
        public class Blink : IntervalAction
        {
            public Blink(float duration, uint count)
                : base(duration)
            {
                blinkPS = 1.0f / (float)count;
                halfBlinkPS = blinkPS / 2.0f;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                renderList = new List<Renderer>();
                originStateList = new List<bool>();
                target.GetComponentsInChildren<Renderer>(renderList);
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    originStateList.Add(render.enabled);
                }
            }

            public override void Stop()
            {
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    render.enabled = originStateList[i];
                }
            }

            protected override void Update()
            {
                float m = percent % (float)blinkPS;
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    render.enabled = (m > halfBlinkPS ? true : false);
                }
            }

            protected float blinkPS = 0;
            protected float halfBlinkPS = 0;
            protected List<Renderer> renderList;
            protected List<bool> originStateList;
        }



        public class ColorBy : IntervalAction
        {
            public ColorBy(float duration, Color deltaColor)
                : base(duration)
            {
                this.deltaColor = deltaColor;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                materialList = new List<Material>();
                startColorList = new List<Color>();
                List<Renderer> renderList = new List<Renderer>();
                target.GetComponentsInChildren<Renderer>(renderList);
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    for (int j = 0; j < render.materials.Length; j++)
                    {
                        Material material = render.materials[j];
                        if (material.HasProperty("_Color"))
                        {
                            Color color = material.color;
                            materialList.Add(material);
                            startColorList.Add(color);
                        }
                    }
                }
            }

            protected override void Update()
            {
                Color dColor = percent * deltaColor;
                for (int i = 0; i < materialList.Count; i++)
                {
                    Material material = materialList[i];
                    Color startColor = startColorList[i];
                    Color color = startColor + dColor;
                    material.color = new Color(
                        Mathf.Clamp01(color.r),
                        Mathf.Clamp01(color.g),
                        Mathf.Clamp01(color.b),
                        Mathf.Clamp01(color.a));
                }
            }

            protected Color deltaColor;
            protected List<Material> materialList;
            protected List<Color> startColorList;
        }


        /// <summary>
        /// Color to
        /// </summary>
        public class ColorTo : IntervalAction
        {
            public ColorTo(float duration, Color toColor)
                : base(duration)
            {
                this.toColor = toColor;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                materialList = new List<Material>();
                startColorList = new List<Color>();
                deltaColorList = new List<Color>();
                List<Renderer> renderList = new List<Renderer>();
                target.GetComponentsInChildren<Renderer>(renderList);
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    for (int j = 0; j < render.materials.Length; j++)
                    {
                        Material material = render.materials[j];
                        if (material.HasProperty("_Color"))
                        {
                            Color color = material.color;
                            Color deltaColor = toColor - color;
                            materialList.Add(material);
                            startColorList.Add(color);
                            deltaColorList.Add(deltaColor);
                        }
                    }
                }
            }

            protected override void Update()
            {
                for (int i = 0; i < materialList.Count; i++)
                {
                    Material material = materialList[i];
                    Color startColor = startColorList[i];
                    Color deltaColor = deltaColorList[i];
                    Color color = startColor + percent * deltaColor;
                    material.color = new Color(
                        Mathf.Clamp01(color.r),
                        Mathf.Clamp01(color.g),
                        Mathf.Clamp01(color.b),
                        Mathf.Clamp01(color.a));
                }
            }

            protected Color toColor;
            protected List<Material> materialList;
            protected List<Color> startColorList;
            protected List<Color> deltaColorList;
        }


        /// <summary>
        /// Fade to
        /// </summary>
        public class FadeTo : IntervalAction
        {
            public FadeTo(float duration, float toOpacity)
                : base(duration)
            {
                this.toOpacity = toOpacity;
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                materialList = new List<Material>();
                startOpacityList = new List<float>();
                deltaOpacityList = new List<float>();
                List<Renderer> renderList = new List<Renderer>();
                target.GetComponentsInChildren<Renderer>(renderList);
                for (int i = 0; i < renderList.Count; i++)
                {
                    Renderer render = renderList[i];
                    for (int j = 0; j < render.materials.Length; j++)
                    {
                        Material material = render.materials[j];
                        if (material.HasProperty("_Color"))
                        {
                            Color color = material.color;
                            float deltaOpacity = toOpacity - color.a;
                            materialList.Add(material);
                            startOpacityList.Add(color.a);
                            deltaOpacityList.Add(deltaOpacity);
                        }
                    }
                }
            }

            protected override void Update()
            {
                for (int i = 0; i < materialList.Count; i++)
                {
                    Material material = materialList[i];
                    Color color = material.color;
                    float startOpacity = startOpacityList[i];
                    float deltaOpacity = deltaOpacityList[i];
                    float opacity = startOpacity + percent * deltaOpacity;
                    material.color = new Color(
                        Mathf.Clamp01(color.r),
                        Mathf.Clamp01(color.g),
                        Mathf.Clamp01(color.b),
                        Mathf.Clamp01(opacity));
                }
            }

            protected float toOpacity;
            protected List<Material> materialList;
            protected List<float> startOpacityList;
            protected List<float> deltaOpacityList;
        }


        /// <summary>
        /// FadeIn
        /// </summary>
        public class FadeIn : FadeTo
        {
            public FadeIn(float duration)
                : base(duration, 1.0f)
            {
            }
        }


        /// <summary>
        /// FadeOut
        /// </summary>
        public class FadeOut : FadeTo
        {
            public FadeOut(float duration)
                : base(duration, 0.0f)
            {
            }
        }

        /// <summary>
        /// Shake
        /// </summary>
        public class Shake: IntervalAction
        {
            public Shake(float duration, float frequency, float x, float y)
                : base(duration)
            {
                this.x = Mathf.Abs(x);
                this.y = Mathf.Abs(y);
                this.z = 0;
                this.frequency = Mathf.Abs(frequency);
            }

            public Shake(float duration, float frequency, float x, float y, float z)
                : base(duration)
            {
                this.x = Mathf.Abs(x);
                this.y = Mathf.Abs(y);
                this.z = Mathf.Abs(z);
                this.frequency = Mathf.Abs(frequency);
            }

            public override void Start(GameObject target)
            {
                base.Start(target);
                startPosition = target.transform.position;
                moveCostTime = frequency + 1;
            }

            protected override void Update()
            {
                // Generate next position
                if(moveCostTime >= frequency)
                {
                    moveCostTime = 0;
                    float dx = Random.Range(-x, x);
                    float dy = Random.Range(-y, y);
                    float dz = Random.Range(-z, z);
                    nextPosition = new Vector3(startPosition.x + dx, startPosition.y + dy, startPosition.z + dz);
                }
                moveCostTime += deltaTime;

                float percent = frequency == 0 ? 1.0f : Mathf.Min(moveCostTime / frequency, 1);
                target.transform.position = Vector3.Lerp(startPosition, nextPosition, percent);
            }

            public override void Stop()
            {
                target.transform.position = startPosition;
            }

            protected float x;
            protected float y;
            protected float z;
            protected float frequency;
            protected Vector3 startPosition;

            private Vector3 nextPosition;
            private float moveCostTime = 0;
        }
    }


    /// <summary>
    /// Action manager
    /// </summary>
    public partial class Action : MonoBehaviour
    {
        void Update()
        {
            Tick(Time.deltaTime);
        }

        public ActionBase Schedule(float time, CallFunc.Callback callback)
        {
            var action = new Sequence(new Delay(time), new CallFunc(callback));
            var repeat = new Repeat(action);
            RunAction(repeat);
            return repeat;
        }

        public ActionBase ScheduleOnce(float time, CallFunc.Callback callback)
        {
            var action = new Sequence(new Delay(time), new CallFunc(callback));
            RunAction(action);
            return action;
        }


        public void UnSchedule(ActionBase action)
        {
            StopAction(action);
        }

        public void RunAction(ActionBase action, GameObject target = null)
        {
            if (target == null) { target = gameObject; }
            action.Start(target);
            AddAction(action);
        }

        public void StopAction(ActionBase action)
        {
            DelAction(action);
        }

        public void ClearAllActions()
        {
            if (isBusy)
            {
                Operation operate = new Operation(OperateType.Clear, null);
                operateList.Add(operate);
            }
            else
            {
                actionList.Clear();
                operateList.Clear();
            }
        }

        protected void Tick(float deltaTime)
        {
            // Tick actions
            isBusy = true;
            LinkedListNode<ActionBase> node = actionList.First;
            while (node != null)
            {
                LinkedListNode<ActionBase> next = node.Next;
                ActionBase action = node.Value;
                action.Step(deltaTime);
                if (action.IsDone())
                {
                    action.Stop();
                    actionList.Remove(node);
                }
                node = next;
            }
            isBusy = false;

            // Deal cache operator
            for(int i=0; i<operateList.Count; ++i)
            {
                Operation cmd = operateList[i];
                switch(cmd.type)
                {
                    case OperateType.Add: { AddAction(cmd.action); break; }
                    case OperateType.Del: { DelAction(cmd.action); break; }
                    case OperateType.Clear: { ClearAllActions(); return; }
                }
            }
            operateList.Clear();
        }

        protected void AddAction(ActionBase action)
        {
            if (isBusy)
            {
                Operation operate = new Operation(OperateType.Add, action);
                operateList.Add(operate);
            }
            else
            {
                actionList.AddLast(action);
            }
        }

        protected void DelAction(ActionBase action)
        {
            if (isBusy)
            {
                Operation operate = new Operation(OperateType.Del, action);
                operateList.Add(operate);
            }
            else
            {
                LinkedList<ActionBase>.Enumerator e = actionList.GetEnumerator();
                while (e.MoveNext())
                {
                    ActionBase curAction = e.Current;
                    if (!Object.ReferenceEquals(curAction, action)) { continue; }
                    curAction.Stop();
                    actionList.Remove(curAction);
                    return;
                }
            }
        }

        protected LinkedList<ActionBase> actionList = new LinkedList<ActionBase>();

        private bool isBusy = false;

        protected List<Operation> operateList = new List<Operation>();


        protected enum OperateType { Add = 0, Del, Clear}
        protected struct Operation
        {
            public Operation(OperateType type, ActionBase action)
            {
                this.type = type;
                this.action = action;
            }
            public OperateType type;
            public ActionBase action;
        }
    }
}


