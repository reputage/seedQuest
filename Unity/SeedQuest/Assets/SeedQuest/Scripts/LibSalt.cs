using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class LibSalt
{

    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern int nacl_randombytes_random();

    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern void nacl_randombytes(byte[] buf, int size);

    // Saves 'size' number of values into buf, determined by 'seed', might be bugged
    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern void nacl_randombytes_buf_deterministic(byte[] buf, int size, byte[] seed);

    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern int nacl_crypto_sign_BYTES();

    // Saves public key to pk, saves secret key to sk
    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern int nacl_crypto_sign_keypair(byte[] pk, byte[] sk);

    // Saves encrypted message 'm' into 'sm', signed with 'sk'
    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern int nacl_crypto_sign(byte[] sm, byte[] m, ulong mlen, byte[] sk);

    // Saves decrypted message 'sm' into 'm", if 'pk' matches the signature on 'sm'
    [DllImport("Assets/Lib/dist/LibSalt.dylib")]
    public static extern int nacl_crypto_sign_open(byte[] m, byte[] sm, ulong smlen, byte[] pk);
}
