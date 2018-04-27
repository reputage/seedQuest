## Just testing
import libnacl
import libnacl.utils
import random

nonce = libnacl.utils.rand_nonce()

random.seed(nonce)
bitVar = random.getrandbits(128)

otpNoKey = libnacl.crypto_generichash(str(bitVar))
otpYesKey = libnacl.crypto_generichash(str(bitVar), nonce)

print(otpNoKey)
print(otpYesKey)