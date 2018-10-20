using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTPworker : MonoBehaviour {
    
    public byte[] otp = new byte[16];
    public byte[] seed = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
    public byte[] key = new byte[16];
    int size = 16;


	private void Start()
	{
        OTPGenerator(otp, size, seed);
        key = OTPxor(seed, otp);

        key = OTPxor(key, otp);

    }

    // Generates a random 16-byte (or 128-bit) seed
    public void randomSeedGenerator()
    {
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = (byte)LibSodiumManager.nacl_randombytes_random();
        }
    }

    // Generates the one-time pad from a seed
	public void OTPGenerator(byte[] otp, int size, byte[] seed)
    {
        LibSodiumManager.nacl_randombytes_buf_deterministic(otp, size, seed);
    }

    // Used to encrypt and decrypt the key using the one-time pad
    public byte[] OTPxor(byte[] key, byte[] otp)
    {
        byte[] result = new byte[key.Length];

        if (key.Length < otp.Length)
        {
            Debug.Log("Error: One time pad is not longer than key");
            return result;
        }

        for (int i = 0; i < key.Length; ++i)
        {
            result[i] = (byte)(key[i] ^ otp[i]);
        }

        return result;
    }

}




/*


vk and sk made from crypto_sign_keypair(seed)
did = makedid(vk)

signature = signresource(body, sk)

    sig = nacl.crypto_sign(body, sk);
    sig = [:nacl.crypto_sign_bytes];
    return keyToKey64u(sig)



From the source code of didery


def makeDid(vk, method="dad"):
    """
    Create and return Did from bytes vk.
    vk is 32 byte verifier key from EdDSA (Ed25519) keypair
    """
    # convert verkey to jsonable unicode string of base64 url-file safe
    vk64u = base64.urlsafe_b64encode(vk).decode("utf-8")
    did = "did:{}:{}".format(method, vk64u)
    return did


def signResource(resource, sKey):
    sig = libnacl.crypto_sign(resource, sKey)
    sig = sig[:libnacl.crypto_sign_BYTES]

    return keyToKey64u(sig)


// From the validate otp blob POSt function:
// takes in the signature of the signer in the header
// and the body of the json object
// and the did:dad id

 def verify(sig, msg, vk):
    """
    Returns True if signature sig of message msg is verified with
    verification key vk Otherwise False
    All of sig, msg, vk are bytes
    """
    try:
        result = libnacl.crypto_sign_open(sig + msg, vk)
    except Exception as ex:
        return False
    return True if result else False


// when about to send a POST request to didery:
// take the signer's signature(?)
// and take the msg (the json body???)
// and use nacl.crypto_sign() to make the vk
// then include the result of nacl.crypto_sign() in the request body(?)

def parseReqBody(req):
    """
    :param req: Falcon Request object
    """
    try:
        raw_json = req.stream.read()
    except Exception as ex:
        raise falcon.HTTPError(falcon.HTTP_400,
                               'Request Error',
                               'Error reading request body.')

    try:
        result_json = json.loads(raw_json, encoding='utf-8')
    except ValueError:
        raise falcon.HTTPError(falcon.HTTP_400,
                               'Malformed JSON',
                               'Could not decode the request body. The '
                               'JSON was incorrect.')

    req.body = result_json
    return raw_json
    

def genOtpBlob(seed=None, changed="2000-01-01T00:00:00+00:00"):
    if seed is None:
        seed = libnacl.randombytes(libnacl.crypto_sign_SEEDBYTES)
    vk, sk = libnacl.crypto_sign_seed_keypair(seed)

    did = h.makeDid(vk)
    body = {
        "id": did,
        "changed": changed,
        "blob": "AeYbsHot0pmdWAcgTo5sD8iAuSQAfnH5U6wiIGpVNJQQoYKBYrPPxAoIc1i5SHCIDS8KFFgf8i0tDq8XGizaCgo9yjuKHHNJZFi0Q"
                "D9K6Vpt6fP0XgXlj8z_4D-7s3CcYmuoWAh6NVtYaf_GWw_2sCrHBAA2mAEsml3thLmu50Dw",
    }

    return vk, sk, did, json.dumps(body, ensure_ascii=False).encode('utf-8')


def makeDid(vk, method="dad"):
    """
    Create and return Did from bytes vk.
    vk is 32 byte verifier key from EdDSA (Ed25519) keypair
    """
    # convert verkey to jsonable unicode string of base64 url-file safe
    vk64u = base64.urlsafe_b64encode(vk).decode("utf-8")
    did = "did:{}:{}".format(method, vk64u)
    return did


from my own curl testing:

curl --header "Content-Type: application/json" 
--header 'Signature: signer="lMAxMfKQFKthpHjTxSP4EBOwPE3nhylt_-wZAJqEWfNNa3i7LiudIFQt9LS_G6W_14aGaGWVFxY3zQPu_pO1AQ=="' 
--request POST 
--data '{"id":"did:dad:fKymS-dKgO3YlfwF5XdCXfx79UN1X22bsM3u9KRxXhY=","blob":"aj;skldfjaoisfjweoijfoiajfo;iasjvjncowrnoiarejnfoj;csacivnfgo;afiewvajdfvo;hnafddjio;ahjfgoia;ehroi;hs","changed":"2018-10-16T16:36:26.511816+00:00"}' 
http://localhost:8080/blob



*/