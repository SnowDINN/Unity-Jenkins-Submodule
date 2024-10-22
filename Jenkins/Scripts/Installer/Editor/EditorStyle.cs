using UnityEditor;
using UnityEngine;

namespace Anonymous.Jenkins
{
	public static class EditorHeader
	{
		private const float alignmentPosition = 12;
		private const float alignmentWidth = 10;
		
		public static void Title(string message, int fontSize)
		{
			var originalFontSize = GUI.skin.label.fontSize;
			var rect = EditorGUILayout.GetControlRect(false);
			rect.x -= 2.5f;
			rect.height += fontSize;

			GUI.skin.label.fontSize = fontSize;
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.Label(rect, message);
			GUI.skin.label.fontStyle = FontStyle.Normal;
			GUI.skin.label.fontSize = originalFontSize;

			EditorGUILayout.Space(fontSize / 1.5f);
		}

		public static void Line(int height)
		{
			var rect = EditorGUILayout.GetControlRect(false, height);
			rect.x -= alignmentPosition;
			rect.width += alignmentWidth;
			rect.height = height;

			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
		}
	}
	
	public static class CategoryHeader
{
	public static bool ShowHeader(GUIContent nameContent, bool foldoutState, string preferenceName = null)
	{
		var height = Styles.categoryHeader.CalcHeight(nameContent, 4000) + 3;

		var rect = GUILayoutUtility.GetRect(1, height - 1);
		rect.width += rect.x;
		rect.x = 0;

		if (Event.current.type == EventType.Repaint)
			Styles.categoryHeader.Draw(rect, nameContent, false, true, true, false);

		rect.x += 14;
		rect.width -= 2;
		var result = EditorGUI.Toggle(rect, foldoutState, Styles.foldoutStyle);

		if (preferenceName != null && result != foldoutState)
			EditorPrefs.SetBool(preferenceName, result);

		return result;
	}

	public static bool ShowHeader(string label, bool foldoutState, string preferenceName = null)
	{
		return ShowHeader(EditorGUIUtility.TrTempContent(label), foldoutState, preferenceName);
	}

	private static class Styles
	{
		public static readonly GUIStyle foldoutStyle;
		public static readonly GUIStyle categoryHeader;

		static Styles()
		{
			var builtInSkin = GetCurrentSkin();
			foldoutStyle = new GUIStyle(EditorStyles.foldout)
			{
				fontStyle = FontStyle.Bold
			};

			categoryHeader = new GUIStyle(builtInSkin.label)
			{
				fontStyle = FontStyle.Bold,
				border =
				{
					left = 2,
					right = 2,
				},
				padding =
				{
					left = 32,
					top = 5,
					bottom = 5
				},
				normal =
				{
					background = GetBackgroundProcedural_Twotone(new Color(0.196f, 0.196f, 0.196f),
						new Color(0.121f, 0.121f, 0.121f))
				}
			};
		}

		private static Texture2D GetBackgroundProcedural(Color color)
		{
			var texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply();
			return texture;
		}

		private static Texture2D GetBackgroundProcedural_Twotone(Color color, Color headerColor)
		{
			const int height = 30;
			var texture = new Texture2D(1, height);
			for (var i = 0; i < height - 1; i++)
				texture.SetPixel(0, i, color);

			texture.SetPixel(0, height - 1, headerColor);
			texture.Apply();
			return texture;
		}

		private static Texture2D GetBackgroundAsset(string assetPath)
		{
			var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
			if (texture)
				return texture;

			Debug.LogWarning("GUITexture Asset does not exist : " + assetPath);
			texture = null;
			return texture;
		}

		private static GUISkin GetCurrentSkin()
		{
			return EditorGUIUtility.isProSkin
				? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene)
				: EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
		}
	}
}
	
	public static class FoldoutHeader
	{
		public static bool ShowHeader(string title, bool display)
		{
			var style = new GUIStyle("ShurikenModuleTitle")
			{
				padding =
				{
					left = 5
				},
				border =
				{
					top = 10,
					bottom = 10,
					left = 5,
					right = 5
				},
				fixedHeight = 25,
				fontSize = 13
			};

			var rect = GUILayoutUtility.GetRect(25, 25, style);
			GUI.Box(rect, title, style);

			var evt = Event.current;
			switch (evt.type)
			{
				case EventType.Repaint:
					EditorStyles.foldout.Draw(new Rect(rect.x + 5.5f, rect.y + 2.5f, 15, 15), false, false, display, false);
					break;
				case EventType.MouseDown when rect.Contains(evt.mousePosition):
					display = !display;
					evt.Use();
					break;
			}

			return display;
		}
	}
}