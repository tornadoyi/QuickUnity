

InputPlatform = {}
InputPlatform.__index = InputPlatform

function InputPlatform.PCRuntime()
	return (Application.platform == RuntimePlatform.WindowsEditor or
		Application.platform == RuntimePlatform.WindowsPlayer or
		Application.platform == RuntimePlatform.OSXEditor or
		Application.platform == RuntimePlatform.OSXPlayer or
		Application.platform == RuntimePlatform.LinuxPlayer)
end

function InputPlatform.MobileRuntime()
	return (Application.platform == RuntimePlatform.IPhonePlayer or
		Application.platform == RuntimePlatform.Android or
		Application.platform == RuntimePlatform.WP8Player)
end


function InputPlatform.WebRuntime()
	return (Application.platform == RuntimePlatform.WindowsWebPlayer or
		Application.platform == RuntimePlatform.OSXWebPlayer)
end

function InputPlatform.GameConsoleRuntime()
	return (Application.platform == RuntimePlatform.PS3 or
		Application.platform == RuntimePlatform.PS4 or
		Application.platform == RuntimePlatform.XBOX360 or
		Application.platform == RuntimePlatform.XboxOne)
end

