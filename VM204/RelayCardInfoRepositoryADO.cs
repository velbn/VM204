﻿using System;
using System.IO;
using System.Collections.Generic;

namespace VM204
{
	public class RelayCardInfoRepositoryADO {
		RelayCardInfoDatabase db = null;
		protected static string dbLocation;		
		protected static RelayCardInfoRepositoryADO me;		

		static RelayCardInfoRepositoryADO ()
		{
			me = new RelayCardInfoRepositoryADO();
		}

		protected RelayCardInfoRepositoryADO ()
		{
			// set the db location
			dbLocation = DatabaseFilePath;

			// instantiate the database	
			db = new RelayCardInfoDatabase(dbLocation);
		}

		public static string DatabaseFilePath {
			get { 
				var sqliteFilename = "RelayCardInfoDatabase.db3";
				#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
				#else

				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif

				#endif
				return path;	
			}
		}

		public static RelayCardInfo GetRelayCardInfo(int id)
		{
			return me.db.GetItem(id);
		}

		public static IEnumerable<RelayCardInfo> GetAllRelayCardInfoItems ()
		{
			return me.db.GetItems();
		}

		public static int SaveRelayCardInfo (RelayCardInfo item)
		{
			return me.db.SaveItem(item);
		}

		public static int DeleteRelayCardInfo(int id)
		{
			return me.db.DeleteItem(id);
		}
	}
}

