﻿using System;
using System.Collections.Generic;
using GridDominance.Graphfileformat.Blueprint;
using GridDominance.Shared.Resources;
using GridDominance.Shared.Screens.WorldMapScreen;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.DeviceBridge;
using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.LogProtocol;

namespace GridDominance.Shared.Screens.Common
{
	public enum WorldUnlockState
	{
		OpenAndUnlocked,                 // World can be entered w/o problems
		ReachableButMustBePreviewed,     // World is reachable but not enough points, show preview
		UnreachableButCanBePreviewed,    // World is unreachable - but still show preview (for IAB / unlock infos)
		UnreachableAndFullyLocked        // World is locked, only show error
	}

	public static class UnlockManager
	{
		public static WorldUnlockState IsUnlocked(GraphBlueprint w, bool showToast) => IsUnlocked(w.ID, showToast);

		public static WorldUnlockState IsUnlocked(Guid id, bool showToast)
		{
			if (id == Levels.WORLD_ID_TUTORIAL)
			{
				return WorldUnlockState.OpenAndUnlocked;
			}

			if (id == Levels.WORLD_001.ID)
			{
				if (MainGame.Inst.Profile.SkipTutorial) return WorldUnlockState.OpenAndUnlocked;
				if (MainGame.Inst.Profile.GetLevelData(Levels.LEVEL_TUTORIAL).HasAnyCompleted()) return WorldUnlockState.OpenAndUnlocked;

				return WorldUnlockState.UnreachableButCanBePreviewed;
			}

			if (id == Levels.WORLD_002.ID)
			{
				switch (MainGame.Flavor)
				{
					case GDFlavor.FREE:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_001, Levels.WORLD_002.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						return WorldUnlockState.UnreachableButCanBePreviewed;
					}

					case GDFlavor.IAB:
					case GDFlavor.IAB_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_001, Levels.WORLD_002.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						if (GetIABState(GDConstants.IAB_WORLD2, Levels.WORLD_002.ID, showToast)) return WorldUnlockState.OpenAndUnlocked;

						return reachable ? WorldUnlockState.ReachableButMustBePreviewed : WorldUnlockState.UnreachableButCanBePreviewed;
					}

					case GDFlavor.FULL:
					case GDFlavor.FULL_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_001, Levels.WORLD_002.ID);

						return reachable ? WorldUnlockState.OpenAndUnlocked : WorldUnlockState.UnreachableAndFullyLocked;
					}

					default:
					{
						SAMLog.Error("UNLCK::EnumSwitch_IU_1", "MainGame.Flavor = " + MainGame.Flavor);
						break;
					}
				}
			}

			if (id == Levels.WORLD_003.ID)
			{
				switch (MainGame.Flavor)
				{
					case GDFlavor.FREE:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_002, Levels.WORLD_003.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						return WorldUnlockState.UnreachableButCanBePreviewed;
					}
					case GDFlavor.IAB:
					case GDFlavor.IAB_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_002, Levels.WORLD_003.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						if (GetIABState(GDConstants.IAB_WORLD3, Levels.WORLD_003.ID, showToast)) return WorldUnlockState.OpenAndUnlocked;

						return reachable ? WorldUnlockState.ReachableButMustBePreviewed : WorldUnlockState.UnreachableButCanBePreviewed;
					}

					case GDFlavor.FULL:
					case GDFlavor.FULL_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_002, Levels.WORLD_003.ID);

						return reachable ? WorldUnlockState.OpenAndUnlocked : WorldUnlockState.UnreachableAndFullyLocked;
					}

					default:
					{
						SAMLog.Error("UNLCK::EnumSwitch_IU_2", "MainGame.Flavor = " + MainGame.Flavor);
						break;
					}
				}
			}

			if (id == Levels.WORLD_004.ID)
			{
				switch (MainGame.Flavor)
				{
					case GDFlavor.FREE:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_003, Levels.WORLD_004.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						return WorldUnlockState.UnreachableButCanBePreviewed;
					}
					case GDFlavor.IAB:
					case GDFlavor.IAB_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_003, Levels.WORLD_004.ID);
						int neededPoints = PointsForUnlock(id);

						if (reachable && MainGame.Inst.Profile.TotalPoints >= neededPoints) return WorldUnlockState.OpenAndUnlocked;

						if (GetIABState(GDConstants.IAB_WORLD4, Levels.WORLD_004.ID, showToast)) return WorldUnlockState.OpenAndUnlocked;

						return reachable ? WorldUnlockState.ReachableButMustBePreviewed : WorldUnlockState.UnreachableButCanBePreviewed;
					}

					case GDFlavor.FULL:
                    case GDFlavor.FULL_NOMP:
					{
						bool reachable = BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_003, Levels.WORLD_004.ID);

						return reachable ? WorldUnlockState.OpenAndUnlocked : WorldUnlockState.UnreachableAndFullyLocked;
					}

					default:
					{
						SAMLog.Error("UNLCK::EnumSwitch_IU_3", "MainGame.Flavor = " + MainGame.Flavor);
						break;
					}
				}
			}

			if (id == Levels.WORLD_ID_GAMEEND)
			{
				if (!BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_004, Levels.WORLD_ID_GAMEEND)) return WorldUnlockState.OpenAndUnlocked;

				return WorldUnlockState.UnreachableAndFullyLocked;
			}

			if (id == Levels.WORLD_ID_MULTIPLAYER)
			{
				switch (MainGame.Flavor)
				{
					case GDFlavor.FREE:
					{
						return WorldUnlockState.UnreachableAndFullyLocked;
					}
					case GDFlavor.IAB:
					{
						return GetIABState(GDConstants.IAB_MULTIPLAYER, Levels.WORLD_ID_MULTIPLAYER, showToast) ? WorldUnlockState.OpenAndUnlocked : WorldUnlockState.ReachableButMustBePreviewed;
					}

					case GDFlavor.FULL:
					{
						return WorldUnlockState.OpenAndUnlocked;
					}

					case GDFlavor.FULL_NOMP:
					case GDFlavor.IAB_NOMP:
					{
						return WorldUnlockState.UnreachableAndFullyLocked;
					}

					default:
					{
						SAMLog.Error("UNLCK::EnumSwitch_IU_4", "MainGame.Flavor = " + MainGame.Flavor);
						break;
					}
				}
			}

			if (id == Levels.WORLD_ID_ONLINE)
			{
				switch (MainGame.Flavor)
				{
					case GDFlavor.FREE:
					{
						return WorldUnlockState.UnreachableAndFullyLocked;
					}
					case GDFlavor.IAB:
					case GDFlavor.IAB_NOMP:
					{
						if (GetIABState(GDConstants.IAB_ONLINE, Levels.WORLD_ID_ONLINE, showToast)) return WorldUnlockState.OpenAndUnlocked;
						if (BlueprintAnalyzer.IsWorldReachable(Levels.WORLD_004, Levels.WORLD_ID_GAMEEND)) return WorldUnlockState.OpenAndUnlocked;

						return WorldUnlockState.ReachableButMustBePreviewed;
					}

					case GDFlavor.FULL:
					case GDFlavor.FULL_NOMP:
					{
						return WorldUnlockState.OpenAndUnlocked;
					}

					default:
					{
						SAMLog.Error("UNLCK::EnumSwitch_IU_5", "MainGame.Flavor = " + MainGame.Flavor);
						break;
					}
				}
			}

			SAMLog.Error("UNLCK::NID", $"UnlockManager: ID not found {id} ({showToast})");
			return WorldUnlockState.UnreachableAndFullyLocked;
		}

		private static bool GetIABState(string iabCode, Guid id, bool toast)
		{
			var ip = MainGame.Inst.GDBridge.IAB.IsPurchased(iabCode);

			if (ip == PurchaseQueryResult.Refunded)
			{
				if (MainGame.Inst.Profile.PurchasedWorlds.Contains(id))
				{
					SAMLog.Debug("Level refunded: " + id);
					MainGame.Inst.Profile.PurchasedWorlds.Remove(id);
					MainGame.Inst.SaveProfile();
				}
				return false;
			}

			if (MainGame.Inst.Profile.PurchasedWorlds.Contains(id)) return true;

			switch (ip)
			{
				case PurchaseQueryResult.Purchased:
					MainGame.Inst.Profile.PurchasedWorlds.Add(id);
					MainGame.Inst.SaveProfile();
					return true;

				case PurchaseQueryResult.NotPurchased:
				case PurchaseQueryResult.Cancelled:
				case PurchaseQueryResult.Pending:
					return false;

				case PurchaseQueryResult.Error:
					if (toast) MainGame.Inst.ShowToast("UNLCK::E1", L10N.T(L10NImpl.STR_IAB_TESTERR), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
					return false;

				case PurchaseQueryResult.Refunded:
					if (MainGame.Inst.Profile.PurchasedWorlds.Contains(id))
					{
						SAMLog.Debug("Level refunded: " + id);
						MainGame.Inst.Profile.PurchasedWorlds.Remove(id);
						MainGame.Inst.SaveProfile();
					}
					return false;

				case PurchaseQueryResult.NotConnected:
					if (toast) MainGame.Inst.ShowToast("UNLCK::E2", L10N.T(L10NImpl.STR_IAB_TESTNOCONN), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
					return false;

				case PurchaseQueryResult.CurrentlyInitializing:
					if (toast) MainGame.Inst.ShowToast("UNLCK::E3", L10N.T(L10NImpl.STR_IAB_TESTINPROGRESS), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
					return false;

				default:
					SAMLog.Error("EnumSwitch_IU", "IsUnlocked()", "MainGame.Inst.Bridge.IAB.IsPurchased(MainGame.IAB_WORLD " + id + ")) -> " + ip);
					return false;
			}
		}

		public static int PointsForUnlock(Guid id)
		{
			if (id == Levels.WORLD_001.ID)
				return 0;

			var sc = (GDConstants.SCORE_DIFF_0 + GDConstants.SCORE_DIFF_1 + GDConstants.SCORE_DIFF_2*0.75f); // (all on D1), (all on D2) and (3/4 on D3)

			int levelcount = 0;
			
			if (id == Levels.WORLD_002.ID || id == Levels.WORLD_003.ID || id == Levels.WORLD_004.ID)
				levelcount += BlueprintAnalyzer.LevelCount(Levels.WORLD_001);
			
			if (id == Levels.WORLD_003.ID || id == Levels.WORLD_004.ID)
				levelcount += BlueprintAnalyzer.LevelCount(Levels.WORLD_002);
			
			if (id == Levels.WORLD_004.ID)
				levelcount += BlueprintAnalyzer.LevelCount(Levels.WORLD_003);

			if (levelcount==0)
			{
			SAMLog.Error("UNLCK::PFU", $"UnlockManager: WorldID not found {id}");
			return 99999;
			}

			return (int)(levelcount * sc);
		}

		public static IEnumerable<Guid> GetFullUnlockState()
		{
			if (IsUnlocked(Levels.WORLD_002, false) == WorldUnlockState.OpenAndUnlocked) yield return Levels.WORLD_002.ID;
			if (IsUnlocked(Levels.WORLD_003, false) == WorldUnlockState.OpenAndUnlocked) yield return Levels.WORLD_003.ID;
			if (IsUnlocked(Levels.WORLD_003, false) == WorldUnlockState.OpenAndUnlocked) yield return Levels.WORLD_002.ID;
			if (IsUnlocked(Levels.WORLD_ID_ONLINE, false) == WorldUnlockState.OpenAndUnlocked) yield return Levels.WORLD_ID_ONLINE;
		}
	}

}
