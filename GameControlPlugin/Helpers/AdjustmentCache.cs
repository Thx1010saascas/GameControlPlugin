namespace Loupedeck.GameControlPlugin.Helpers
{
    public static class AdjustmentCache
    {
        private static readonly object Lock = new object();
        private static readonly DictionaryNoCase<AdjustmentCacheEntry> Cache = new DictionaryNoCase<AdjustmentCacheEntry>();

        public static void AddEntry(string pluginName, uint stickId, string axisName, string actionParameter)
        {
            lock (Lock)
            {
                string key = GetKey(stickId, axisName);
                
                if(!Cache.ContainsKey(key))
                    Cache.Add(key, new AdjustmentCacheEntry()
                    {
                        PluginName = pluginName,
                        ActionParameter = actionParameter
                    });
            }
        }
       
        public static AdjustmentCacheEntry Get(uint stickId, string axisName)
        {
            string key = GetKey(stickId, axisName);

            if (Cache.TryGetValue(key, out AdjustmentCacheEntry entry))
                return entry;

            return null;
        }
        
        private static string GetKey(uint stickId, string axisName)
        {
            return $"{stickId}_{axisName}";
        }
    }

    public class AdjustmentCacheEntry
    {
        public string PluginName { get; set; }
        public string ActionParameter { get; set; }
    }
}