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
		cls.__parent   = super
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
		return cls.__parents[parent.__fullname] ~= nil
	end

	local function instantiate(parent, ...)
		local instance = {}
		for k,v in pairs(cls) do instance[k] = v end

		if parent ~= nil then
			setmetatable(instance, {
				__index = parent, 
				__newindex = function ( t, k, v )
					if parent[k] ~= nil then 
						parent[k] = v
						return
					end
					rawset(instance, k, v)
				end,
				__tostring = function() return string.format("%s [instance]", classname) end})
			instance.__base = parent
		end
		instance.__class = cls
		if instance.ctor then instance:ctor(...) end
		return instance
	end

	cls.CreateByLuaComponent = function (component)	
        return instantiate(component)
	end

	setmetatable(cls, {
		__index = super, 
		__call = function ( t, ... )
			local parent = super and super(...) or nil
			return instantiate(parent, ...)
		end,
		__tostring = function() return string.format("%s [class]", classname) end})

	return cls
end	

