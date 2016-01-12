## WalkTo Command

This is a Plug&Play staff command. Just drop this script anywhere inside your Scripts folder and it's ready to go.

To use it, login with a staff account, type `[WalkTo`, target a NPC then target the spots where you want the creature to walk to, in sequence. Press ESC when you finish creating the path.

Each time you target a spot a waypoint is added, linked to the previous waypoint.

Those waypoints are self-deleted after one minute, by default. You can change this delay on the Config class.

While walking through the waypoints, the NPC Current Speed is changed to 0.2, to make them walk smoothly. After the last waypoint gets deleted, the NPC Current Speed is set back to its Passive Speed.

All waypoints are deleted when the shard restarts, just to make sure you won't have any lost waypoints around.

You can use this command in several NPCs simultaneously.