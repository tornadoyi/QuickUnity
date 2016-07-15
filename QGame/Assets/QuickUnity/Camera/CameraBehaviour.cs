using UnityEngine;
using System.Collections;


namespace QuickUnity
{
    public class CameraBehaviour : MonoBehaviour
    {

        public virtual void Look(Vector3 center)
        {
            transform.LookAt(center);
        }

        public virtual void Move(float x, float y, float z)
        {
            transform.Translate(x, y, z);
        }

        public virtual void RotateHorizontal(float angle)
        {
            Vector3 curEulerAngle = transform.eulerAngles;
            transform.eulerAngles = new Vector3(curEulerAngle.x, curEulerAngle.y + angle, curEulerAngle.z);
        }

        public virtual void RotateVertical(float angle)
        {
            if (!verticalLimited)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);
            }
            else
            {
                Vector3 eulerAngles = transform.eulerAngles;

                Vector3 normal_old_min = Vector3.Cross(transform.forward, minVerticalLimit);
                Vector3 normal_old_max = Vector3.Cross(transform.forward, maxVerticalLimit);

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);

                Vector3 normal_new_min = Vector3.Cross(transform.forward, minVerticalLimit);
                Vector3 normal_new_max = Vector3.Cross(transform.forward, maxVerticalLimit);

                if (normal_old_min.x * normal_new_min.x < 0 ||
                    normal_old_max.x * normal_new_max.x < 0)
                {
                    transform.eulerAngles = eulerAngles;
                }
            }
        }

        public virtual void Zoom(float unit)
        {
            transform.Translate(0, 0, unit);
        }

        public virtual void RotateAroundHorizontal(Vector3 center, float angle)
        {
            transform.RotateAround(center, Vector3.up, angle);
        }

        public virtual void RotateAroundVertical(Vector3 center, float angle)
        {
            if (!verticalLimited)
            {
                transform.RotateAround(center, transform.right, angle);
            }
            else
            {
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;

                Vector3 direct_old = center - transform.position;
                Vector3 normal_old_min = Vector3.Cross(direct_old, minVerticalLimit);
                Vector3 normal_old_max = Vector3.Cross(direct_old, maxVerticalLimit);

                transform.RotateAround(center, transform.right, angle);

                Vector3 direct_new = center - transform.position;
                Vector3 normal_new_min = Vector3.Cross(direct_new, minVerticalLimit);
                Vector3 normal_new_max = Vector3.Cross(direct_new, maxVerticalLimit);

                if (normal_old_min.x * normal_new_min.x < 0 ||
                    normal_old_max.x * normal_new_max.x < 0)
                {
                    transform.position = position;
                    transform.rotation = rotation;
                }
            }
        }


        // Properties
        public bool verticalLimited = true;
        public Vector3 minVerticalLimit = new Vector3(0, -1, 0);
        public Vector3 maxVerticalLimit = new Vector3(0, 1, 0);

    }
}


