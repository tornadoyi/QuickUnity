

Formula = {}
Formula.__index = Formula

-- ====================================== Define ====================================== --
Formula.SphereAddPercentByHit = 0.15
Formula.CritProbability = 0
Formula.CritDamageRatio = 1.5
Formula.XPDamageRatio = 3

--[[
	Property counteract 
]]
Formula.PropertyCounteract = {
	{ 1,  0.5,  1.5, 1.2, 1.2},
	{ 1.5, 1, 0.5, 1.2, 1.2},
	{ 0.5, 1.5, 1, 1.2, 1.2},
	{ 0.8, 0.8, 0.8, 1, 1.5 },
	{ 0.8, 0.8, 0.8, 1.5, 1 },
}

--[[
	Chain damage ratio 
]]
Formula.ChainDamageRatio = { [0] = 1, [1] = 1.1, [2] = 1.2, [3] = 1.2, [4] = 1.2, [5] = 1.5 }

-- ====================================== Battle ====================================== --

--[[
	MP recover value when round comming
]]
function Formula.GetMPRecoverPerRound(maxMP) return maxMP * 0.15 end


--[[
	MP recover when character be hitted
]]
function Formula.GetMPRecoverByHit(damage, maxHP, maxMP) return math.min(0.1 * maxMP, damage/2*maxHP) end


--[[
	Chain damage ratio
]]
function Formula.GetChainDamageRatio(chain) 
	if chain > 5 then chain = 5 end
	if chain < 1 then chain = 1 end
	return Formula.ChainDamageRatio[chain]
end	

--[[
	Damage fomula
]]
function Formula.GetDamageByHit(attack, chain, srcPID, dstPID, isCrit, isXP) 
	
	-- Get chain ratio
	local chainRatio = Formula.GetChainDamageRatio(chain) 

	-- Get property counteract
	local propertyRatio = Formula.PropertyCounteract[srcPID][dstPID]

	-- Crit ratio
	local critRatio = isCrit and Formula.CritDamageRatio or 1

	-- XP ratio
	local xpRatio = isXP and Formula.XPDamageRatio or 1

	-- Random ratio
	local randomRatio = 1 + math.random(-50, 50) / 1000

	-- Calculate
	return math.ceil(attack * chainRatio * propertyRatio * critRatio * xpRatio * randomRatio)
end