using UnityEngine;
using System;
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
}
