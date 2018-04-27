## OTPing
import libnacl
import libnacl.utils
import random

def unbytify(b=bytearray([]), reverse=False):
    """
    Returns unsigned integer equivalent of bytearray b
    b may be any iterable of ints including bytes or list of ints.
    Big endian is the default.
    If reverse is true then it reverses the order of the bytes in the byte array
    before conversion for little endian.
    """
    b = bytearray(b)
    if not reverse:  # process MSB first on the right
        b.reverse()
    n = 0
    while b:
        n <<= 8
        n += b.pop()
    return n

def generateOTP(strVar=None, nonce=None):
    """
    libnacl.crypto_generichash() uses the blake2 hash function:
    http://libnacl.readthedocs.io/en/latest/topics/raw_generichash.html

    There's a version of blake2 called blake2X which outputs longer hashes,
    however blake2X does not seem to be included in libnacl.

    Takes a string as a parameter, and a nonce as an optional parameter, returns
    blake2 hash of the string.

    Example:
    generateOTP("quick brown fox")
    """
    if not strVar:
        print("String required to generate OTP.")
        return 0
    if nonce:
        otp = libnacl.crypto_generichash(str(strVar), nonce)
    else:
        otp = libnacl.crypto_generichash(str(strVar))
    return otp