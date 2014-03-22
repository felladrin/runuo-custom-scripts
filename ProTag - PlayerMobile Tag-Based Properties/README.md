## Introduction ##

Similar to _ArteGordon's XmlAttachment_ and _Tru's PlayerMobile Helper_, this is an alternative way to save your custom PlayerMobile properties.

## Features ##

- Plug &amp; Play
- Works out of the box for any script
- In-game manageable
- Easy customization for the tag prefix
- Does not involve Serialize/Deserialize
- Clean Reset/Uninstall

## Installation ##

Just drop it somewhere on your Scripts folder.

## In-game Commands ##

> **[ProTag**<br/>
Gets all the custom properties of a targeted PlayerMobile.

> **[ProTag Set**<br/>
Sets a custom property for a targeted PlayerMobile.

> **[ProTag Get**<br/>
Gets the value of an espefic custom property of a targeted PlayerMobile.

> **[ProTag Del**<br/>
Deletes an espefic custom property of a targeted PlayerMobile.

> **[ProTag ClearAll**<br/>
Used to remove all account tags created by the ProTag System. You should use this command before deleting the script, if you are uninstalling. Note that this is the only command _case sensitive_, for security reasons.

## Development Usage ##

Considering "**pm**" as a PlayerMobile object and "**Max Wisdom**" your custom property, you can use ProTag in your script like this:

To set his Max Wisdom to 85:

	ProTag.Set(pm, "Max Wisdom", "85"); 

To get his Max Wisdom and store it on a variable:

	string maxWisdom = ProTag.Get(pm, "Max Wisdom");

To delete his Max Wisdom property.

	ProTag.Del(pm, "Max Wisdom");

## Some Notes ##

In most cases you won't need to add any "using" statement on your script header to use the ProTags. But if you ever need, just add "_using Server;_".

Your custom property can have more than one word. When set to the tag name, all blank spaces will be replaced by underscores.

All the properties are stored as String, so when you use ProTag.Get to recover its value, you will need to parse it to your desired type. For example:

	string maxWisdom = ProTag.Get(pm, "Max Wisdom");
	int max = int.Parse(maxWisdom); 