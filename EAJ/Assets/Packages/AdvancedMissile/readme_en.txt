<<Version>>
3.2.1

<<Version changes>>
1.1
・Add the missile model.
・Add the burner effect.
・Add the "offset" of effect.

1.2.1
・Add item "Direction" to "Movement".
・Partial name change.
・Update the value of some samples of missile.

2.2.1
・Partial name change.
・Bug fix.
・Add preprocessor "#if UNITY_EDITOR" to using statement.
・Add avoidance function(Item:Avoidance).
・Add line collider function.(Item:Collision > Enable Liner Collider).
・Add item "RigidBody".

3.0.0
・Partial name change.
・Partial processing change.
・Overall UI change.
・Specification changed dramatically. You can now select only the processing you want to execute with a checkbox.
・Add fuse function (Item: Fuse)
・Fixed to generate avoidance route not only in the horizontal direction but also in the vertical direction
・DEMO change

<<How to use>>
1. Attach "AdvancedMissile.cs" to the object you want to missile.
2. Adjust the parameters.
3. You can confirmation of the missile behaviour.

<<Parameters>>
・Destroy
To set about the destruction of the missiles.

	- MinDestroyTime
	The minimum time to missile destruction.

	- MaxDestroyTime
	The maximum time to missile destruction.

	- LowPowerFall
	Enable fall.

	-- FallStartTime
	Elapsed time until the fall start (seconds).

	-- Drag
	Air resistance when the fall (Fall will slow the larger the value).
	It will affect the RigidBody.

・Fuse
To set about the fuse.

	-FuseType
	Setting of fuse type

	--SuperQuick
	Destroy immediately after collide
	
	--Time
	Destroy after set time

	---Time
	Time to blow up

	--Proximity
	Destroy at distance with target

	---Distance
	Distance to blow up

	--Height
	Destroy at altitude

	---HeightType
	Type to detect altitude

	----Position
	Detected by coordinates

	----Raycast
	Detected by raycast

	-----LayerMask
	Layers not to detect raycast

	----- Height
	Altitude to blow up

・Movement
To set about the movement of the missiles.

	- AnglePerSecond
	Angle of rotation per second.

	- Type
	Movement method.

	- Direction
	Direction of movement.

	-- Translate
	Move in the direction and distance.

	--- MinSpeed
	Minimum speed.

	--- MaxSpeed
	Maximum speed.

	-- Addforce
	Add force to Rigidbody.

	--- ForceMode
	Setting of force.

	--- MinPower
	Minimum power.

	--- MaxPower
	Maximum power.

	-RotateType
	Setting of rotate type

	--NoRotate
	No rotate

	--PerSecond
	Angle per second

	---Angle
	Angle rotate

	--Torque
	Rotate by torque

	---Torque
	Torque power

・Search
To set about the search for targets.

	- StartFollowInterval
	Time until the start of the follow(seconds).

	- SerachType
	Retrieval method.

	-- TargetTags
	List tag as a target.

	-- TargetNames
	List name as a target.

	-IgnoreNearestObject
	Ignore the nearest of object.

	- CanReseach
	During follow, Enable to again search the goal.
	The closest is selected object.

	-- Interval
	Time until again search(seconds).

	- SightAngle
	The field of view of the missile.
	If out of field of view, missile fly to forwards.
	Left and right is the X Axis.
	Up and down is the Y Axis.

・Offset
To set about the gap of the target position.

	- Offset
	Gap of the target position.
	Rather than the player's feet, used when you want the waist to a target.

	- IsRandomOffset
	Enable the target position to random.

	-- Amplitude
	The size of the gap.

	-- OffsetX, Y, Z
	The extent of the gap corresponding to the distance between the target
	(from 0 to 1).
	・Horizontal axis	: distance
	・Vertical axis		: degree of gap

	-- MinInterval
	The minimum time to calculate the target position again (seconds).

	-- MaxInterval
	The maximum time to calculate the target position again (seconds).

・Collision
To set about the collision of the missiles.

	- EnableCollision
	Enable the collision.

	-- EnableInterval
	Time until enable the collision(seconds).

	-- ColliderEachOther
	Enable collision missiles each other.

	--EnableLinerCollider
	Enable the line collision.

	---Radius
	Radius of capsule ley

・Avoidance
	-CanAvoid
	Activate the avoidance function.

	--DistanceBetweenObstacle
	Distance of between the obstacle and route.

	--ReCreateInterval
	Time until again create the route.(seconds).

	--CreateUpRoute
	Whether to generate an upward route.
	
	--CreateDownRoute
	Whether to generate an downward route.

	--DrawRoute
	Draw the route to scene view.
 
・Audios
To set about the Sound effects.

	- ShotSE
	Sound effect at the time of the missile launch.

	-- Volume
	Volume of sound effect at the time of the missile launch.

	- ExplosionSE
	Sound effect at the time of the missile explosion.

	-- Volume
	Volume of sound effect at the time of the missile explosion.

・Effects
To set about the Missile effects.

	- SmokeEffect
	Smoke effect emitting from the missile (which can be in the beam).

	--Offset
	To set about the gap of the emmit position.

	-- KeepChild
	When the missile is destroyed, maintain the effect in the state of the children of the missile.
	If keep child, at the same time disappear with the missile destruction.

	--- DestroyTime
	Time until destroy the effect (seconds).

	- ExplosionEffect
	Explosion effect object.

	- LocalScale
	Local scale of effect object.
 
・Event
To set about call of the event.

	- CallMethod
	Name of the function to call.

	- SendValueType
	Type of argument.

	-- Value
	Value of argument.


<<Demo>>
https://www.youtube.com/watch?v=CLC2xMn3a4s

<<Contact>>
Twitter:@isemito_niko