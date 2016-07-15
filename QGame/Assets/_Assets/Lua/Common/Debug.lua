


local function _OnOff(onOff)
	if not Application.isEditor then return false end
	return onOff
end


GameDebug = {
	Battle = {
		StateChange 		= _OnOff(false),
		ShowOrderList		= _OnOff(false),
	},

	Character = {
		PropertyChange		= _OnOff(false),

	}
}






