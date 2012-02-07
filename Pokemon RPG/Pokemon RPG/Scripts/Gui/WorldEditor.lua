require "LuaModules\\Base"

local menu = gui.registerMenu("worldEditor")
local canvas = menu:AddCanvas("mainCanvas")

local wldEditLabel = canvas:AddClickableLabel("Back to main menu", nil, Vector2.Zero, Color.White)
wldEditLabel.Position = Vector2(game.getWidth() - wldEditLabel.Size.Width - 10, 10)

wldEditLabel.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		game.setState("mainMenu")
	end
end)

--local btnTest = canvas:AddRectangleButton("Test Button", Vector2(10, 10), Color(68, 65, 64), Color.White, Size(110, 25))