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


def generateOTP(strVar=None, randKey=None):
    """
    libnacl.crypto_generichash() uses the blake2 hash function:
    http://libnacl.readthedocs.io/en/latest/topics/raw_generichash.html

    There's a version of blake2 called blake2X which outputs longer hashes,
    however blake2X does not seem to be included in libnacl.

    Takes a string as a parameter, and a random key as an optional 
    parameter, returns blake2 hash of the string.

    Example:
    generateOTP("quick brown fox")
    generateOTP("quick brown fox", "lazy dog")
    """
    if not strVar:
        print("String required to generate OTP.")
        return 1
    if randKey:
        otp = libnacl.crypto_generichash(str(strVar), randKey)
    else:
        otp = libnacl.crypto_generichash(str(strVar))
    return otp


def longOTP(strVar=None, chunks=2):
    """
    The output of blake2 might not be long enough as is for the 
    one time pad, so this funciton concatenates the output of 
    blake2 multiple times to create a longer otp.

    The otp needs to be reproducible, so keys used from 0 to integer 
    "chunks" are used to generate the chunks of the otp.

    Example:
    longOTP("quick brown fox", 3)
    """
    longPad = []
    if not strVar:
        print("String required to generate OTP.")
        return 1
    elif chunks <= 0:
        print("Number of chunks must be more than zero.")
        return 2
    else:
        for i in range(0, chunks):
            otp = generateOTP(strVar, str(i))
            otp = unbytify(otp)
            longPad.append(otp)
    return longPad

