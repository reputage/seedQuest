# This is still in the works

import os, sys
from os import path

sys.path.append(path.dirname(path.dirname(path.abspath(__file__))))

from src import otping
import libnacl
import libnacl.utils
import random


nonce = libnacl.utils.rand_nonce()
random.seed(nonce)
bitVar = random.getrandbits(128)
strVar = "quick brown fox"

otpNoNonce = generateOTP(strVar)
otpNonce = generateOTP(strVar, nonce)
otpBits = generateOTP(bitVar)
otpFaulty = generateOTP()

print(optNoNonce)
print(otpNonce)
print(otpBits)
print(otpFaulty)