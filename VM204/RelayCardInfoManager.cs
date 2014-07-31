using System;
using System.Collections.Generic;

namespace VM204
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class RelayCardInfoManager
	{


		public static RelayCardInfo GetRelayCardInfo(int id)
		{
			return RelayCardInfoRepositoryADO.GetRelayCardInfo(id);
		}

		public static IList<RelayCardInfo> GetAllRelayCardInfos ()
		{
			return new List<RelayCardInfo>(RelayCardInfoRepositoryADO.GetAllRelayCardInfoItems());
		}

		public static int SaveRelayCardInfo (RelayCardInfo item)
		{
			return RelayCardInfoRepositoryADO.SaveRelayCardInfo(item);
		}

		public static int DeleteRelayCardInfo(int id)
		{
			return RelayCardInfoRepositoryADO.DeleteRelayCardInfo(id);
		}
	}
}

