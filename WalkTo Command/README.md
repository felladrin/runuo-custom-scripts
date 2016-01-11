## WalkTo Command

This is a Plug&Play staff command. Just drop this script anywhere inside you Scripts folder and it's ready to go.

To use it, just login with a staff account, type `[WalkTo`, target a NPC then target the spots where you want the creature to walk to, in sequence. Press ESC when you finish creating the path.

Each time you target a spot a waypoint is added, linked to the previous waypont.

Those waypoints self-delete themselves after one minute, by default. You can change this delay on the Config class.

All waypoints are deleted when the shard restarts, just to make sure you won't have any lost waypoints around.