#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private IBrowsable DrawBrowser(List<IBrowsable> allItems, IBrowsable currentItem)
		{
			var selected = default(IBrowsable);
			var columns  = Mathf.FloorToInt((position.width - 28) / Settings.IconSize);

			if (columns > 0)
			{
				var rows  = Mathf.CeilToInt(allItems.Count / (float)columns);
				var index = 0;

				for (var r = 0; r < rows; r++)
				{
					EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						for (var c = 0; c < columns; c++)
						{
							if (index < allItems.Count)
							{
								var item = allItems[index++];

								if (item != null)
								{
									var rect = DrawIcon(Settings.IconSize, item.GetIcon(), item.GetTitle(), item == currentItem);

									if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) == true)
									{
										if (Event.current.button == 0)
										{
											selected = item;
										}
										else
										{
											CwHelper.SelectAndPing(item.GetObject());
										}
									}
								}
								GUILayout.FlexibleSpace();
							}
						}
					EditorGUILayout.EndHorizontal();
				}
			}

			return selected;
		}

		private static GUIStyle titleStyleA;
		private static GUIStyle titleStyleB;
		private static GUIStyle titleStyleC;

		private static GUIStyle GetTitleBold()
		{
			if (titleStyleC == null)
			{
				titleStyleC = new GUIStyle(EditorStyles.boldLabel);
				titleStyleC.alignment = TextAnchor.MiddleCenter;
			}

			return titleStyleC;
		}

		private static GUIStyle GetTitleStyle(bool selected)
		{
			if (selected == true)
			{
				if (titleStyleA == null)
				{
					titleStyleA = new GUIStyle(EditorStyles.miniLabel);
					titleStyleA.alignment = TextAnchor.MiddleCenter;
				}

				return titleStyleA;
			}
			else
			{
				if (titleStyleB == null)
				{
					titleStyleB = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
					titleStyleB.alignment = TextAnchor.MiddleCenter;
				}

				return titleStyleB;
			}
		}

		private static Rect DrawIcon(int size, Texture2D icon, string name, bool selected, string title = null)
		{
			EditorGUILayout.BeginVertical(GUILayout.Width(size));
				if (string.IsNullOrEmpty(title) == false)
				{
					GUILayout.Label(title, GetTitleBold(), GUILayout.Width(size));
				}
				var rect = EditorGUILayout.BeginVertical(GUILayout.Width(size), GUILayout.Height(size));
					if (icon != null)
					{
						GUI.DrawTexture(rect, icon);
					}
					else
					{
						GUI.DrawTexture(rect, Texture2D.whiteTexture);
					}
					GUILayout.Label(new GUIContent(default(Texture), name), GetSelectableStyle(selected, false), GUILayout.Width(size), GUILayout.Height(size));
				EditorGUILayout.EndVertical();

				GUILayout.Label(name, GetTitleStyle(selected), GUILayout.Width(size));
			EditorGUILayout.EndVertical();

			return rect;
		}
	}
}
#endif