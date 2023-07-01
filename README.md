Game Control Plugin for Loupedeck
Version 0.7

INTRODUCTION
The original code is by Mikal Shaikh. See https://forum.dcs.world/topic/305865-new-loupedeck-plugin/#comment-5160957. In that forum he indicated that I was able to make changes and republish them.

I (Steven White) made some changes including:
1. Reduce duplicate code.
2. Add the ability to override the default vJoy device id that is specified in the settings. This lets us have access multiple vJoy devices so we can have as many buttons and encoders as we want.
3. Consolidated all the adjustments into a single Loupedeck action.
4. Added the ability for an axis to press one of two buttons. An UpButton or a DownButton.

OVERVIEW:

Allows the Loupedeck to act as a game controller by sending DirectX buttons to a virtual joystick via vJoy.

PREREQUISITES:
-	Please download and install the latest version of vJoy available here: https://sourceforge.net/projects/vjoystick/
-	After installing vJoy, please configure vJoy by creating at least one device with all axes on, 128 buttons, and 4 POV hats (in "4 Directions" mode)

INSTALLING THE PLUGIN:
-	Double click the provided GameControlPlugin.lplug4 and the Loupedeck software will ask to install it. 

USAGE:

See https://github.com/Thx1010saascas/GameControlPlugin/wiki for a concise set of examples of most of the available actions.

- 	After installing the plugin a settings.txt file will be created in %USERPROFILE%\AppData\Local\Loupedeck\PluginData\GameControl.  The key setting is
	on the first line, which determines which vJoy device the plugin will use (default is device 1).  The rest of the settings are the default values that
	the plugin will use for new actions.  In general they should be left as-is.

-	In the LoupeDeck software, the plugin shows a list of the various actions that are available -- button presses, various types of toggles, and various axis functions.

-	Each of these actions contains a Name field (which is only used to identify which action is which in the UI) and a free text entry field (which is where all the meat
	of the plugin is and which I will refer to as the "Action Settings" field).

-	The general format of the Action Settings field is "[[action value]] ; [[option name]] = [[option value]]> ; [[option name]] = [[option value]]> ; ..."

-	The only entry that every action MUST have is the [[action value]].  The options are totally optional, and you can have as many or as few as you like.  If you simply
	enter the value with no options, the default values from settings.txt will be used for any options.
	
-   As mentioned above, the default vJoy device id specified in the settings file. To override the vJoy id, add the command "vJoyID=X" in the options field.
    An example is "vJoyId=2;dn=on,f". This means use vJoy device #2 (instead of the default), display rotary encoder numbers in the Loupedeck device and use use "fast" number incrementing.    

ACTION VALUES:

The [[action value]] field must be one of the following:

-	An integer value from 1 to 128 that is a DirectX button (for buttons and toggles)
-	An integer value from 1 to 1000 or "Fast", "Normal", "Slow", which are all adjustment speeds (for the rotary axis adjustment actions)
-	An integer value from -100 to 100 that represents a percentage of the axis to change to. "Min", "Center" and "Max" may also be usedto reset the axis
-	POV actions accept values such as "Up", "Down", "Left" or "Right".

Notes: 	(i) the quotation marks above are simply for readability and should NOT be inserted in any fields
        (ii) all actions are case insensitive, and in most cases abbreviations are accepted (e.g., "norm" or "n" for "Normal")


OPTIONS:

The [[action name]] field, along with their associated [[option values]] are below:

-	"Label" creates a text label for the action that is drawn on the button.  The [[option value]] is any text you want, without quotes.  For example, "Label = Landing Gear"

-	"LabelBackgroundColor" sets the color of the box drawn behind the label. Valid [[option values]] are "Black", "Blue", "Red", "Green", "Gray", "Purple", "White" 
	or "None" (ie, no background).  In addition, [[action value]] can be a custom color by inserting a decimal number representing a packed ARGB value.  

-	"LabelColor" sets the color of the label text.  All the same [[option values]] for LabelBackgroundColor are valid. Note that due to software limitations, text colors
	other than white tend to look a bit thin.

-	"LabelSize" sets the font size of the labels in points.  While technically any integer can be inserted, values between 10 and 28 are recommended. Note that this is an
	experimental option and some minor formatting strangeness can occur when modifying the font size.

-	"LabelPos" determines the vertical position of the label. [[option values]] can be "Top", "Bottom", "Center" or an integer (generally between 1 and 80) for a custom position.

-	"ButtonType" allows you to choose the image used for the various button actions.  [[option values]] can be "Gray Square", "Black Square", "Red Round", "Gray Round", 
	"Black Round", "Left", "Right", "Up", "Down", "Clockwise" or "Counterclockwise". In general, these images are based on real-life aircraft buttons.

-	"RotaryType" allows you to choose the image used for the various button actions. The only [[option value]] permitted at this time is "Gray".  More will be added in
	in later versions. 

-	"DrawNumbers" turns off or on the automatic drawing of DX button numbers on the image.  [[option values]] are "True" (or "On" or "Yes") and "False" (or "Off" or "No").

-	"DrawToggleIndicators" turns off or on the drawing of toggle indicators.  [[option values]] are "True" (or "On" or "Yes") and "False" (or "Off" or "No").

-	"DXSendType" allows you to set a toggle to either a hold operation (where the dx button is constantly pressed) or a pulsed operation (where the dx button is sent for 50 
	milliseconds). [[option values]] are "Pulsed" or "Hold".  Note that this option only has effect for the "Toggle (On-On)" and "Toggle (On-On-On)" actions.

-	"ToggleAsButton" is a special option that only applies to the "Toggle (On-Off)" action. When enabled, the image for the toggle is replaced with a button -- determined by 
	the ButtonType option.  When the toggle is in the off state, the button image is grayed out, indicating that it is off.  [[option values]] are [[option values]] are "True" 
	(or "On" or "Yes") and "False" (or "Off" or "No").

-   "DefaultValue" can be used to default an axis to a specific percentage value on startup. EG: You might want some radios off, loud, etc. 

Notes: 	(i) the quotation marks above are simply for readability and should NOT be inserted in any fields, (ii) all actions are case insensitive, and in most cases 
		abbreviations are accepted (e.g., "l" for "Label", "bt" for "ButtonType" or "ccw" for "Counterclockwise")


EXAMPLES:
-	A button that looks like an F-16 ICP ALOW button and sends DX button 4 could be created by using a "Button Press" action and the following Action Settings field:

	"4 ; Label = ALOW/2 ; ButtonType = Gray Square; LabelPos = Center ; LabelBackgroundColor = None; LabelSize = 11; DrawNumbers = False"

	or abbreviated as follows:

	"4 ; Label=ALOW/2 ; bt=gs; lp=c ; lbc=None;ls=11; dn=False"


-	A toggle that acts as hold for a landing gear (using DX buttons 23 and 24) might be:

	"23 ; Label = Landing Gear ; LabelBackgroundColor = Blue ; DrawNumbers = False ; DXSendType = Hold"

	or abbreviated as follows:

	"23 ; L = Landing Gear ; LBC = Blue ; DN = False ; DX = H"



THIS SOFTWARE IS PROVIDED "AS IS", AND THE AUTHORS MAKES NO WARRANTIES, EXPRESS OR IMPLIED, AND HEREBY DISCLAIMS ALL IMPLIED WARRANTIES.