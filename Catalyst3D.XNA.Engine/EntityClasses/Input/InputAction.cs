using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Catalyst3D.XNA.Engine.EntityClasses.Input
{
	public class InputAction
	{
		public Buttons Button;
		public Keys Key;
		public string Name;
		public bool IsPressed;
		public float PressTime;
		public float PressPower;
		public GestureType GestureType;

		public InputAction(string name, Buttons btn, Keys key)
		{
			Name = name;
			Button = btn;
			Key = key;
		}

		public InputAction(string name, GestureType type)
		{
			Name = name;
			GestureType = type;
		}
	}
}