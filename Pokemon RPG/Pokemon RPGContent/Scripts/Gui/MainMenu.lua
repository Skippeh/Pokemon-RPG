require "LuaModules\\Base"

local margin = 5
local offsetBottom = 80

local menu = gui.registerMenu("MainMenu")

-- Main menu --
local mainMenu = menu:AddCanvas("mainCanvas")

-- Background texture
mainMenu:AddPicture(io.loadTexture("img\\menu\\bg"), Vector2.Zero, Size(game.getWidth(), game.getHeight()), Color.White)

local pokeLogo = io.loadTexture("img\\menu\\poke_logo")
local logoSize = Vector2(pokeLogo.Width, pokeLogo.Height)

local logo = mainMenu:AddPicture(pokeLogo, game.getCenter(logoSize), Size.Empty, Color.White)
logo.Position = Vector2(logo.Position.X, logo.Position.Y * 0.5)

local btnExit = mainMenu:AddClickableLabel("Quit", nil, Vector2.Zero, Color(85, 85, 105))
btnExit.Position = Vector2(game.getCenter(Vector2(btnExit.Size.Width, btnExit.Size.Height)).X, game.getHeight() - (offsetBottom + btnExit.Size.Height + margin))
btnExit.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		menu:SetCanvas("promptExitCanvas")
	end
end)

local btnOptions = mainMenu:AddClickableLabel("Options", nil, Vector2.Zero, Color(85, 85, 105))
btnOptions.Position = Vector2(game.getCenter(Vector2(btnOptions.Size.Width, btnOptions.Size.Height)).X, btnExit.Position.Y - btnExit.Size.Height - margin)
btnOptions.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		menu:SetCanvas("optionsCanvas")
	end
end)

local btnWorldEdit = mainMenu:AddClickableLabel("World Editor", nil, Vector2.Zero, Color(85, 85, 105))
btnWorldEdit.Position = Vector2(game.getCenter(Vector2(btnWorldEdit.Size.Width, 0)).X, btnOptions.Position.Y - btnOptions.Size.Height - margin)
btnWorldEdit.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		game.setState("worldEditor")
	end
end)

local btnPlay = mainMenu:AddClickableLabel("Play", nil, Vector2.Zero, Color(85, 85, 105))
btnPlay.Position = Vector2(game.getCenter(Vector2(btnPlay.Size.Width, 0)).X, btnWorldEdit.Position.Y - btnWorldEdit.Size.Height - margin)
btnPlay.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		-- Change to a screen where you can select to load, or start a new game, etc.
	end
end)
-- End of main menu --

-- Options menu --
local optionsMenu = menu:AddCanvas("optionsCanvas")

-- Background texture
optionsMenu:AddPicture(io.loadTexture("img\\menu\\bg"), Vector2.Zero, Size(game.getWidth(), game.getHeight()), Color.White)

local backBtn = optionsMenu:AddClickableLabel("Back", nil, Vector2(10, 20), Color(85, 85, 105))
backBtn.Position = btnExit.Position
backBtn.Size.Width = 200
backBtn.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		menu:SetCanvas("mainCanvas")
	end
end)
-- End of options menu --

-- Exit prompt menu--
local promptExitCanvas = menu:AddCanvas("promptExitCanvas")

-- Background texture
promptExitCanvas:AddPicture(io.loadTexture("img\\menu\\bg"), Vector2.Zero, Size(game.getWidth(), game.getHeight()), Color.White)

local logo = promptExitCanvas:AddPicture(pokeLogo, game.getCenter(logoSize), Size.Empty, Color.White)
logo.Position = Vector2(logo.Position.X, logo.Position.Y * 0.5)

local lblPrompt = promptExitCanvas:AddLabel("Are you sure?", nil, Vector2.Zero, Color(85, 85, 105))
lblPrompt.Position = Vector2(game.getCenter(Vector2(lblPrompt.Size.Width, 0)).X, btnPlay.Position.Y)

local btnYes = promptExitCanvas:AddClickableLabel("Yes", nil, Vector2.Zero, Color(85, 85, 105))
btnYes.Position = Vector2(game.getCenter(Vector2(btnYes.Size.Width, 0)).X, lblPrompt.Position.Y + lblPrompt.Size.Height + margin)

btnYes.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		game.exit()
	end
end)

local btnNo = promptExitCanvas:AddClickableLabel("No", nil, Vector2.Zero, Color(85, 85, 105))
btnNo.Position = Vector2(game.getCenter(Vector2(btnNo.Size.Width, 0)).X, btnYes.Position.Y + btnYes.Size.Height + margin)

btnNo.MouseUp:Add(function(obj, args)
	if args.ContainedMouse and args.Button == MouseButtons.Left then
		menu:SetCanvas("mainCanvas")
	end
end)
--End of exit prompt menu --

-- Set the current menu to the main menu.
menu:SetCanvas("mainCanvas")