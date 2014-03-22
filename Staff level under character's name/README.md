## Instructions ##

This is a fast mod to let your players know who is officially member of the staff.

![[IMG]](http://i.imgur.com/qp6srIr.png)

All you need to do is to open **PlayerMobile.cs** and look for the line:


    base.GetProperties(list);

Then, **under** this line you **add** the following:

	if (AccessLevel > AccessLevel.Player)
	    list.Add(1060847, "{0}\t{1}", "Shard", Enum.GetName(typeof(AccessLevel), AccessLevel));     

That's it. Restart and you'll see all staff members accordingly identified by their levels.