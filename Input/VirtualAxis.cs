﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;


namespace Nez
{
	/// <summary>
	/// A virtual input represented as a float between -1 and 1
	/// </summary>
	public class VirtualAxis : VirtualInput
	{
		public List<Node> nodes = new List<Node>();

		public float value
		{
			get
			{
				foreach( var node in nodes )
				{
					var value = node.value;
					if( value != 0 )
						return value;
				}

				return 0;
			}
		}


		public VirtualAxis() : base()
		{}


		public VirtualAxis( params Node[] nodes )
		{
			this.nodes.AddRange( nodes );
		}


		public override void update()
		{
			foreach( var node in nodes )
				node.update();
		}


		static public implicit operator float( VirtualAxis axis )
		{
			return axis.value;
		}


		#region Node types

		public abstract class Node : VirtualInputNode
		{
			public abstract float value { get; }
		}


		public class GamePadLeftStickX : Node
		{
			public int gamepadIndex;
			public float deadzone;


			public GamePadLeftStickX( int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE )
			{
				this.gamepadIndex = gamepadIndex;
				this.deadzone = deadzone;
			}

			public override float value
			{
				get
				{
					return Mathf.signThreshold( Input.gamePads[gamepadIndex].getLeftStick( deadzone ).X, deadzone );
				}
			}
		}


		public class GamePadLeftStickY : Node
		{
			public int gamepadIndex;
			public float deadzone;


			public GamePadLeftStickY( int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE )
			{
				this.gamepadIndex = gamepadIndex;
				this.deadzone = deadzone;
			}

			public override float value
			{
				get
				{
					return Mathf.signThreshold( Input.gamePads[gamepadIndex].getLeftStick( deadzone ).Y, deadzone );
				}
			}
		}


		public class GamePadRightStickX : Node
		{
			public int gamepadIndex;
			public float deadzone;


			public GamePadRightStickX( int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE )
			{
				this.gamepadIndex = gamepadIndex;
				this.deadzone = deadzone;
			}

			public override float value
			{
				get
				{
					return Mathf.signThreshold( Input.gamePads[gamepadIndex].getRightStick( deadzone ).X, deadzone );
				}
			}
		}


		public class GamePadRightStickY : Node
		{
			public int gamepadIndex;
			public float deadzone;


			public GamePadRightStickY( int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE )
			{
				this.gamepadIndex = gamepadIndex;
				this.deadzone = deadzone;
			}

			public override float value
			{
				get
				{
					return Mathf.signThreshold( Input.gamePads[gamepadIndex].getRightStick( deadzone ).Y, deadzone );
				}
			}
		}


		public class GamePadDpadLeftRight : Node
		{
			public int gamepadIndex;


			public GamePadDpadLeftRight( int gamepadIndex = 0 )
			{
				this.gamepadIndex = gamepadIndex;
			}


			public override float value
			{
				get
				{
					if( Input.gamePads[gamepadIndex].DpadRightDown )
						return 1f;
					else if( Input.gamePads[gamepadIndex].DpadLeftDown )
						return -1f;
					else
						return 0f;
				}
			}
		}


		public class GamePadDpadUpDown : Node
		{
			public int gamepadIndex;


			public GamePadDpadUpDown( int gamepadIndex = 0 )
			{
				this.gamepadIndex = gamepadIndex;
			}


			public override float value
			{
				get
				{
					if( Input.gamePads[gamepadIndex].DpadDownDown )
						return 1f;
					else if( Input.gamePads[gamepadIndex].DpadUpDown )
						return -1f;
					else
						return 0f;
				}
			}
		}


		public class KeyboardKeys : Node
		{
			public OverlapBehavior overlapBehavior;
			public Keys positive;
			public Keys negative;

			private float _value;
			private bool turned;


			public KeyboardKeys( OverlapBehavior overlapBehavior, Keys negative, Keys positive )
			{
				this.overlapBehavior = overlapBehavior;
				this.negative = negative;
				this.positive = positive;
			}


			public override void update()
			{
				if( Input.getKeyDown( positive ) )
				{
					if( Input.getKeyDown( negative ) )
					{
						switch( overlapBehavior )
						{
							default:
							case OverlapBehavior.CancelOut:
								_value = 0;
								break;

							case OverlapBehavior.TakeNewer:
								if( !turned )
								{
									_value *= -1;
									turned = true;
								}
								break;
							case OverlapBehavior.TakeOlder:
								//value stays the same
								break;
						}
					}
					else
					{
						turned = false;
						_value = 1;
					}
				}
				else if( Input.getKeyDown( negative ) )
				{
					turned = false;
					_value = -1;
				}
				else
				{
					turned = false;
					_value = 0;
				}
			}


			public override float value
			{
				get { return _value; }
			}
		}

		#endregion

	}
}
