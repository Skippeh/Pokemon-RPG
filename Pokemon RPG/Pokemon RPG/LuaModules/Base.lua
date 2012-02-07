--Uncategorized
luanet.load_assembly("Pokemon_RPG")

-- XNA types
luanet.load_assembly("Microsoft.Xna.Framework")
Vector2 	= luanet.import_type("Microsoft.Xna.Framework.Vector2")
Color		= luanet.import_type("Microsoft.Xna.Framework.Color")
Rectangle 	= luanet.import_type("Microsoft.Xna.Framework.Rectangle")
InGame 		= luanet.import_type("Microsoft.Xna.Framework.Game")

-- GUI types
luanet.load_assembly("XNAGui")
Button 	= luanet.import_type("XNAGui.Button")
Canvas 	= luanet.import_type("XNAGui.Canvas")
Control = luanet.import_type("XNAGui.Control")
PositionChangedEventArgs = luanet.import_type("XNAGui.PositionChangedEventArgs")
TextBox = luanet.import_type("XNAGui.TextBox")
ToolTip = luanet.import_type("XNAGui.ToolTip")
Size 	= luanet.import_type("XNAGui.Size")
Picture = luanet.import_type("XNAGui.Picture")
Label 	= luanet.import_type("XNAGui.Label")

-- Windows forms
luanet.load_assembly("System.Windows.Forms")
MouseButtons = luanet.import_type("System.Windows.Forms.MouseButtons")

-- Game states
luanet.load_assembly("Pokemon_RPG.States")
states.State 		= luanet.import_type("Pokemon_RPG.States.State")
states.InGame 		= luanet.import_type("Pokemon_RPG.States.InGame")
states.Intro 		= luanet.import_type("Pokemon_RPG.States.Intro")
states.MainMenu 	= luanet.import_type("Pokemon_RPG.States.MainMenu")
states.WorldEditor 	= luanet.import_type("Pokemon_RPG.States.WorldEditor")