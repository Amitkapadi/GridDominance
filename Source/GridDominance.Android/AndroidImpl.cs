using Android.OS;
using MonoSAMFramework.Portable.DeviceBridge;
using System.Text;

namespace GridDominance.Android
{
	class AndroidImpl : IOperatingSystemBridge
	{
		public FileHelper FileHelper { get; } = new AndroidFileHelper();

		public string FullDeviceInfoString { get; } = GenerateInfoStr();
		public string DeviceName { get; } = string.Format("{0} {1}", Build.Manufacturer, Build.Model);
		public string DeviceVersion { get; } = string.Format("Android {0} sdk-{1}", Build.VERSION.Release, Build.VERSION.Sdk);

		private static string GenerateInfoStr()
		{
			StringBuilder b = new StringBuilder();

			b.AppendFormat("VERSION.Codename    := '{0}'\n", Build.VERSION.Codename);
			b.AppendFormat("VERSION.Incremental := '{0}'\n", Build.VERSION.Incremental);
			b.AppendFormat("VERSION.Release     := '{0}'\n", Build.VERSION.Release);
			b.AppendFormat("VERSION.Sdk         := '{0}'\n", Build.VERSION.Sdk);
			b.AppendFormat("VERSION.SdkInt      := '{0}'\n", Build.VERSION.SdkInt);
			b.AppendFormat("Board               := '{0}'\n", Build.Board);
			b.AppendFormat("Bootloader          := '{0}'\n", Build.Bootloader);
			b.AppendFormat("Brand               := '{0}'\n", Build.Brand);
			b.AppendFormat("CpuAbi              := '{0}'\n", Build.CpuAbi);
			b.AppendFormat("CpuAbi2             := '{0}'\n", Build.CpuAbi2);
			b.AppendFormat("Device              := '{0}'\n", Build.Device);
			b.AppendFormat("Display             := '{0}'\n", Build.Display);
			b.AppendFormat("Fingerprint         := '{0}'\n", Build.Fingerprint);
			b.AppendFormat("Hardware            := '{0}'\n", Build.Hardware);
			b.AppendFormat("Host                := '{0}'\n", Build.Host);
			b.AppendFormat("Id                  := '{0}'\n", Build.Id);
			b.AppendFormat("Manufacturer        := '{0}'\n", Build.Manufacturer);
			b.AppendFormat("Model               := '{0}'\n", Build.Model);
			b.AppendFormat("Product             := '{0}'\n", Build.Product);
			b.AppendFormat("Radio               := '{0}'\n", Build.Radio);
			b.AppendFormat("RadioVersion        := '{0}'\n", Build.RadioVersion);
			b.AppendFormat("Serial              := '{0}'\n", Build.Serial);
			b.AppendFormat("Tags                := '{0}'\n", Build.Tags);
			b.AppendFormat("Time                := '{0}'\n", Build.Time);
			b.AppendFormat("Type                := '{0}'\n", Build.Type);
			b.AppendFormat("User                := '{0}'\n", Build.User);

			return b.ToString();
		}
	}
}