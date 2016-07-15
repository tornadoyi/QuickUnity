
Serial = class("Serial")

function Serial:ctor( high, low )
	self.high = high
	self.low = low
end

function Serial:__tostring() return string.format("%s_%s", self.high, self.low) end

function Serial:__eq(other)
	if other == nil or other.__cname ~= "Serial" then
		GError("Invalid arguments")
		return false
	end	
	return (self.high == other.high and self.low == other.low)
end

function Serial:__lt(other)
	if other == nil or other.__cname ~= "Serial" then
		GError("Invalid arguments")
		return false
	end	
	print(self.high, self.low, other.high, other.low)
	if self.high > other.high then return false end
	if self.high < other.high then return true end
	return self.low < other.low
end

function Serial:__le(other)
	if other == nil or other.__cname ~= "Serial" then
		GError("Invalid arguments")
		return false
	end	
	print(self.high, self.low, other.high, other.low)
	if self.high > other.high then return false end
	if self.high < other.high then return true end
	return self.low <= other.low
end

