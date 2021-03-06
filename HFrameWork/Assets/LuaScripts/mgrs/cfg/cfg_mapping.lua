local cfgMapping = {
	path = {
		city = "cfg/t_city",
		hero = "cfg/t_hero",
		instancedata = "cfg/t_instancedata",
		item = "cfg/t_item",
		npcdata = "cfg/t_npcdata",
		skill = "cfg/t_skill",
	},
	cfgs = {
		city = {
			 value = function(key) return require("cfg/t_city").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
		hero = {
			 value = function(key) return require("cfg/t_hero").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
		instancedata = {
			 value = function(key) return require("cfg/t_instancedata").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
		item = {
			 value = function(key) return require("cfg/t_item").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
		npcdata = {
			 value = function(key) return require("cfg/t_npcdata").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
		skill = {
			 value = function(key) return require("cfg/t_skill").templete end,
			 pair = function(callback) end,
			 num = function() end,
		},
	},
}

return cfgMapping