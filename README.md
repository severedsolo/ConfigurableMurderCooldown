# ConfigurableMurderCooldown
A mod to configure the default cooldown between murders in Shadows of Doubt

A spiritual successor to MurderCooldownAccelerator but hopefully a little less fragile (and a little more performant).

# Features
- Random functions use the standard .Net Random functions rather than the in game ones. **Hopefully** this should mean they won't break between updates.
- Ability to set separate cooldowns for new murderers (before their first kill) and subsequent kills. Defaults to between 12 and 48 hours for a new murderer and 1 and 12 for a second kill (the idea is that you shouldn't feel like there is a murderer on the loose constantly, but when they become active they will move quite quickly).
- Fixed mode is still available, just set the same value in both xxxKillerMinCooldown and xxxKillerMaxCooldown.
- Cooldown is disabled for the first murder of a new game (as it is in banilla) but this can be overridden by changing the config option.
