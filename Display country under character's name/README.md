## Display country under character's name

This script displays the player country based on their client's language code.

![](screenshot.png)

### Install

Download `Country.cs` and drop it anywhere inside your scripts folder.

Then open `PlayerMobile.cs` and find:

    public override void GetProperties(ObjectPropertyList list)
    {
        base.GetProperties(list);

**Above** that lines, add:

    #region Display country under character name
    string m_Country;
    bool m_TriedToGetCountry;
    public string Country
    {
        get
        {
            if (!m_TriedToGetCountry && m_Country == null)
            {
                m_Country = Felladrin.Utilities.Country.GetNameFromCode(Language);
                m_TriedToGetCountry = true;
            }
    
            return m_Country;
        }
    }
    #endregion

**Below** that lines, add:

    #region Display country under character name
    if (Country != null)
    {
        list.Add(1060658, "{0}\t{1}", "From", Country);
    }
    #endregion

### Uninstall

Just remove the two blocks of code you've added, and delete `Country.cs`.