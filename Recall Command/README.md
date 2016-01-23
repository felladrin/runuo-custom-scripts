# Recall Command

This is a plug&play player command to teleport them back to a fixed location, generally the hot point of the shard, where it's safe to be AFK or to logout. In my shard it's often used to regroup after raids, and it's located near the shard starting point, so veteran players can quickly meet newcomers if needed.

## Configuration

There are some configs you can change on the script, to suit your needs:

    bool Enabled = true;                                       // Is this command enabled?
    bool AllowUsageIfIsOverloaded = true;                      // Should we allow players to use this command if they are overloaded?
    bool AllowUsageIfIsInCombat = true;                        // Should we allow players to use this command if they are in combat?
    bool DecreaseFameIfUsedInCombat = true;                    // Should we decrease the player fame if he use this command to flee from combat?
    bool SpecialEffects = true;                                // Should we use special effects after teleporting the player?
    bool BringFollowers = true;                                // Should we also teleport the player's followers?
    bool AffectOnlyControlledFollowers = true;                 // Should we only teleport the player's followers that are controlled?
    Map TargetMap = Map.Trammel;                               // To what map should we teleport the player?
    Point3D TargetLocation = new Point3D(1434, 1702, 9);       // To what coordinates should we teleport the player?
