// TODO: limit packet to < Steam limit (currently 512 * 1024 = ~524k)
// see: https://partner.steamgames.com/doc/api/steamnetworkingtypes#messages

namespace FutzSys {
	public static class FutzConst {

		public const string REGISTER_HOST_PACKET_ID = "03";
		public const int CUSTOM_PACKET_START = 10;
		public const int SYSTEM_AGENT_ID = 515;

	}
}