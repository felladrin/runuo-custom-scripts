# Set Skill Cap

This is a plug&play script to automate the skill cap changes of your shard.

It would be easy to increase total skill cap for all players using the built-in command line "[global set skillcap 1300 where playermobile", but it wouldn't be easy to decrease, or change the individual skill cap foreach player. That's why I've written this script.

When a player logs in, it makes sure the player skill caps are correctly. For example: if before the restart the skill cap was 1200 and after the restart it was 700, it will remove skill points of those players who have more than 700 total skill points. The smart remove will remove first focus and meditation, as they are easy to regain. Then the less used skills (skills with less points). So in the end, players should be with their best skills intact.

I've used it on my shard when I was still evaluating what would be the best skill cap.

# Install

Just drop this script anywhere inside your Scripts folder.

# Configuration

Open the script and change the Config to your needs:

    bool Enabled = true;             // Is this system enabled?
    int IndividualSkillCap = 120;    // Cap for each skill. Default: 100.
    int TotalSkillCap = 1200;        // Cap for the sum of all skills. Default: 700.
