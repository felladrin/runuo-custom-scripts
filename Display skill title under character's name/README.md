## Display skill title under character's name

This is a fast mod to display the skill title under character's name. It's the same skill title displayed on paperdoll.

![[]](screenshot.png)

All you need to do is to open **PlayerMobile.cs** and look for the line:

    base.GetProperties(list);

Then, **below** this line you **add** the following:

	if (AccessLevel == AccessLevel.Player)
	{
        string skillTitle = Titles.GetSkillTitle(this);
        string titleStart = skillTitle.Substring(0, skillTitle.IndexOf(' '));
        string titleEnd = skillTitle.Substring(skillTitle.IndexOf(' ') + 1);
        list.Add(1060847, "{0}\t{1}", titleStart, titleEnd);
	}

That's it. Restart and you'll see the skill title below every player character's names.