using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Cyberfuck
{
	public static class Input
	{
		public static MouseState mouseState;
		public static MouseState lastMouseState;

		public static KeyboardState keyboardState;
		public static KeyboardState lastKeyboardState;

		public static bool IsActive { get; private set; }

		public static void Update(bool isActive)
		{
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
			UpdateScrollWheel();

			lastKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();

			Input.IsActive = isActive;
		}


		#region Key Presses

		public static bool KeyWentDown(Keys key)
		{
			return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
		}

		public static bool KeyWentUp(Keys key)
		{
			return lastKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
		}

		#endregion


		#region Keyboard State

		public static bool IsKeyDown(Keys key)
		{
			return keyboardState.IsKeyDown(key);
		}

		public static bool IsKeyUp(Keys key)
		{
			return keyboardState.IsKeyUp(key);
		}

		public static bool Shift
		{
			get { return keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift); }
		}

		public static bool Control
		{
			get { return keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl); }
		}

		public static bool Alt
		{
			get { return keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt); }
		}

		#endregion


		#region Mouse State

		public static bool LeftMouseWentDown
		{
			get { return IsActive && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed; }
		}

		public static bool LeftMouseWentUp
		{
			get { return IsActive && lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released; }
		}

		public static bool LeftMouseIsDown
		{
			get { return IsActive && mouseState.LeftButton == ButtonState.Pressed; }
		}

		public static bool LeftMouseIsUp
		{
			get { return !IsActive || mouseState.LeftButton == ButtonState.Released; }
		}


		public static bool RightMouseWentDown
		{
			get { return IsActive && lastMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed; }
		}

		public static bool RightMouseWentUp
		{
			get { return IsActive && lastMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released; }
		}

		public static bool RightMouseIsDown
		{
			get { return IsActive && mouseState.RightButton == ButtonState.Pressed; }
		}

		public static bool RightMouseIsUp
		{
			get { return !IsActive || mouseState.RightButton == ButtonState.Released; }
		}


		public static bool MiddleMouseWentDown
		{
			get { return IsActive && lastMouseState.MiddleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed; }
		}

		public static bool MiddleMouseWentUp
		{
			get { return IsActive && lastMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released; }
		}

		public static bool MiddleMouseIsDown
		{
			get { return IsActive && mouseState.MiddleButton == ButtonState.Pressed; }
		}

		public static bool MiddleMouseIsUp
		{
			get { return !IsActive || mouseState.MiddleButton == ButtonState.Released; }
		}


		public static Point MousePosition
		{
			get { return new Point(mouseState.X, mouseState.Y); }
		}

		public static Point LastMousePosition
		{
			get { return new Point(lastMouseState.X, lastMouseState.Y); }
		}


		public static Point LeftMouseDrag
		{
			get
			{
				if(IsActive && mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed)
					return new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y);
				else
					return new Point();
			}
		}

		public static Point MiddleMouseDrag
		{
			get
			{
				if(IsActive && mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton == ButtonState.Pressed)
					return new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y);
				else
					return new Point();
			}
		}

		public static Point RightMouseDrag
		{
			get
			{
				if(IsActive && mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Pressed)
					return new Point(mouseState.X - lastMouseState.X, mouseState.Y - lastMouseState.Y);
				else
					return new Point();
			}
		}

		#endregion


		#region Mouse Scroll

		public static int scrollWheelAccumulate;
		public static bool ScrollWheelClickUp { get; private set; }
		public static bool ScrollWheelClickDown { get; private set; }
		public static int ScrollWheelClick { get; private set; }

		private static void UpdateScrollWheel()
		{
			ScrollWheelClickUp = ScrollWheelClickDown = false;
			ScrollWheelClick = 0;

			scrollWheelAccumulate += (mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue);

			if(scrollWheelAccumulate >= 120)
			{
				ScrollWheelClickUp = true;
				ScrollWheelClick = 1;
				scrollWheelAccumulate -= 120;
			}
			if(scrollWheelAccumulate <= -120)
			{
				ScrollWheelClickDown = true;
				ScrollWheelClick = -1;
				scrollWheelAccumulate += 120;
			}
		}

		#endregion


		#region Keyboard Movement

		public static Point SimpleKeyboardMovement
		{
			get
			{
				Point move = Point.Zero;
				if(keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
					move.X -= 1;
				if(keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
					move.X += 1;
				if(keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
					move.Y -= 1;
				if(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
					move.Y += 1;
				return move;
			}
		}

		#endregion

		public static string InputString(string text)
		{
			Keys[] pressedKeys = keyboardState.GetPressedKeys();
			Keys[] lastPressedKeys = lastKeyboardState.GetPressedKeys();

			foreach (var key in pressedKeys)
			{
				bool down = true;
				foreach (var lastKey in lastPressedKeys)
				{
					if (lastKey == key)
						down = false;
				}
				if (down)
				{
					text = InputKey(key, text);
				}
			}
			return text;
		}
		/// <summary>
		/// Inputs a key to the textbox.
		/// </summary>
		/// <param name="Key">The key to input.</param>
		public static string InputKey(Keys Key, string text)
		{
			if (Key == Keys.Back)
			{
				if (text.Length > 0)
					text = text.Substring(0, text.Length - 1);

				return text;
			}

			var Char = KeyToChar(Key, Shift);

			if (Char != 0)
				text += Char;
			return text;
		}

		/// <summary>
		/// Converts a key to a char.
		/// </summary>
		/// <param name="Key">They key to convert.</param>
		/// <param name="Shift">Whether or not shift is pressed.</param>
		/// <returns>The key in a char.</returns>
		public static char KeyToChar(Keys Key, bool Shift = false)
		{
			/* It's the space key. */
			if (Key == Keys.Space)
			{
				return ' ';
			}
			else
			{
				string String = Key.ToString();

				/* It's a letter. */
				if (String.Length == 1)
				{
					Char Character = Char.Parse(String);
					byte Byte = Convert.ToByte(Character);

					if (
						(Byte >= 65 && Byte <= 90) ||
						(Byte >= 97 && Byte <= 122)
						)
					{
						return (!Shift ? Character.ToString().ToLower() : Character.ToString())[0];
					}
				}

				/* 
				 * 
				 * The only issue is, if it's a symbol, how do I know which one to take if the user isn't using United States international?
				 * Anyways, thank you, for saving my time
				 * down here:
				 */

				#region Credits :  http://roy-t.nl/2010/02/11/code-snippet-converting-keyboard-input-to-text-in-xna.html for saving my time.
				switch (Key)
				{
					case Keys.D0:
						if (Shift) { return ')'; } else { return '0'; }
					case Keys.D1:
						if (Shift) { return '!'; } else { return '1'; }
					case Keys.D2:
						if (Shift) { return '@'; } else { return '2'; }
					case Keys.D3:
						if (Shift) { return '#'; } else { return '3'; }
					case Keys.D4:
						if (Shift) { return '$'; } else { return '4'; }
					case Keys.D5:
						if (Shift) { return '%'; } else { return '5'; }
					case Keys.D6:
						if (Shift) { return '^'; } else { return '6'; }
					case Keys.D7:
						if (Shift) { return '&'; } else { return '7'; }
					case Keys.D8:
						if (Shift) { return '*'; } else { return '8'; }
					case Keys.D9:
						if (Shift) { return '('; } else { return '9'; }

					case Keys.NumPad0: return '0';
					case Keys.NumPad1: return '1';
					case Keys.NumPad2: return '2';
					case Keys.NumPad3: return '3';
					case Keys.NumPad4: return '4';
					case Keys.NumPad5: return '5';
					case Keys.NumPad6: return '6';
					case Keys.NumPad7: return '7'; ;
					case Keys.NumPad8: return '8';
					case Keys.NumPad9: return '9';

					case Keys.OemTilde:
						if (Shift) { return '~'; } else { return '`'; }
					case Keys.OemSemicolon:
						if (Shift) { return ':'; } else { return ';'; }
					case Keys.OemQuotes:
						if (Shift) { return '"'; } else { return '\''; }
					case Keys.OemQuestion:
						if (Shift) { return '?'; } else { return '/'; }
					case Keys.OemPlus:
						if (Shift) { return '+'; } else { return '='; }
					case Keys.OemPipe:
						if (Shift) { return '|'; } else { return '\\'; }
					case Keys.OemPeriod:
						if (Shift) { return '>'; } else { return '.'; }
					case Keys.OemOpenBrackets:
						if (Shift) { return '{'; } else { return '['; }
					case Keys.OemCloseBrackets:
						if (Shift) { return '}'; } else { return ']'; }
					case Keys.OemMinus:
						if (Shift) { return '_'; } else { return '-'; }
					case Keys.OemComma:
						if (Shift) { return '<'; } else { return ','; }
				}
				#endregion

				return (Char)0;

			}
		}
    }
}
