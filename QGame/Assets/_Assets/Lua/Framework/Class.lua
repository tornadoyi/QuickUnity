function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

--Create an class.
function class(classname, super)
	-- check
	if type(classname) ~= "string" then print(string.format("invalid class name %s", type(classname))) return end
	if super and type(super) ~= "table" then
        print("Invalid super type %s", type(super))
		super = nil
    end
	
	-- define class
	local cls = nil
	if super then
		cls         = clone(super)
		cls.__parent   = super.__parent
		cls.__parents	= cls.__parent or {}
		cls.__parents[super.__fullname] = super
	else
		cls = {}
	end
	
	-- set cls
	cls.__fullname = classname
	--cls.__index = cls

	cls.subclass = function(parent)
		if parent == nil then return end
		return cls.supers[parent.__fullname] ~= nil
	end

	cls.new = function (...)
		local instance = setmetatable({}, cls)
		instance.class = cls
		if instance.ctor then instance:ctor(...) end
		return instance
	end
	
	cls.CreateByLuaComponent = function (component)	
		local instance = {}
		for k,v in pairs(cls) do instance[k] = v end
        setmetatable(instance, {
        	__index = component,
        	__newindex = function ( t, k, v )
					if component[k] ~= nil then 
						component[k] = v
						return
					end
					rawset(instance, k, v)
				end})

        instance.__base = component
        if instance.ctor then instance:ctor() end
        return instance
	end

	setmetatable(cls, {
		__index = super, 
		__call = function ( t, ... )
			-- body
			local inst_super = super(...)
			local instance = {}
			for k,v in pairs(cls) do instance[k] = v end
			setmetatable(instance, {
				__index = inst_super, 
				__newindex = function ( t, k, v )
					if inst_super[k] ~= nil then 
						inst_super[k] = v
						return
					end
					rawset(instance, k, v)
				end})
			instance.__base = inst_super
			return instance
		end})

	return cls
end	



function class_inherit_c( classname, super )
	-- check
	if type(classname) ~= "string" then print("invalid class name %s", type(classname)) return end
	if super == nil then print("super must not be nil") return end

	-- body
	local cls = {__fullname = classname}
	local smt = getmetatable(super)
	setmetatable(cls, {
		__index = super, 
		__call = function ( t, ... )
			-- body
			local inst_super = super(...)
			local instance = {}
			for k,v in pairs(cls) do instance[k] = v end
			setmetatable(instance, {
				__index = inst_super, 
				__newindex = function ( t, k, v )
					-- body
					if inst_super[k] ~= nil then 
						inst_super[k] = v
						return
					end
					rawset(instance, k, v)
				end})
			return instance
		end})
	return cls
end

