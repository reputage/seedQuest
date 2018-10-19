using UnityEngine;
using System;
using System.Text;
using System.Runtime.InteropServices;

public class LibSodiumManager : MonoBehaviour {

    private static LibSodiumManager instance = null;
    public static LibSodiumManager Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<LibSodiumManager>();
            return instance;
        }
    }

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern int nacl_randombytes_random();

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern void nacl_randombytes(byte[] buf, int size);

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern void nacl_randombytes_buf_deterministic(byte[] buf, int size, byte[] seed);

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern int nacl_crypto_sign_BYTES();

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern int nacl_crypto_sign_keypair(byte[] pk, byte[] sk);

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern int nacl_crypto_sign(byte[] sm, byte[] m, ulong mlen, byte[] sk);

    [DllImport("Assets/Lib/libsodium_wrapper.dylib")]
    public static extern int nacl_crypto_sign_open(byte[] m, byte[] sm, ulong smlen, byte[] pk);

	void Start () {
        //Test();
        //test2();
	}
	 
    void Test()
    {
        Debug.Log("LibSodium Test: randombytes_buf_deterministic");

        int size = 16;
        byte[] buf = new byte[size];
        byte[] seed = new byte[] {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a,
            0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
            0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f
          };
        LibSodiumManager.nacl_randombytes_buf_deterministic(buf, size, seed);

        Debug.Log("Buffer Size: " + buf.Length);
        Debug.Log("Buffer: " + ByteArrayToString(buf));
    }

    public static string ByteArrayToString(byte[] ba)
    {
        return BitConverter.ToString(ba).Replace("-", "");
    }

    public void test2()
    {
        string message_string = "message";

        byte[] message = Encoding.ASCII.GetBytes(message_string);

        byte[] pk = new byte[32];
        byte[] sk = new byte[64];

        nacl_crypto_sign_keypair(pk, sk);

        byte[] pk2 = new byte[32];
        byte[] sk2 = new byte[64];

        nacl_crypto_sign_keypair(pk2, sk2);

        int signed_bytes = nacl_crypto_sign_BYTES();

        byte[] signed_message = new byte[signed_bytes + message.Length];

        nacl_crypto_sign(signed_message, message, (ulong)message.Length, sk);

        byte[] unsigned_message = new byte[message.Length];

        int success = nacl_crypto_sign_open(unsigned_message, signed_message, (ulong)signed_message.Length, pk);

        if (success == 0)
            Debug.Log("Correct signature");
        else
            Debug.Log("Incorrect signature");

        int success2 = nacl_crypto_sign_open(unsigned_message, signed_message, (ulong)signed_message.Length, pk2);

        if (success2 == 0)
            Debug.Log("Correct signature");
        else
            Debug.Log("Incorrect signature");
    }

}
