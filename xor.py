## XOR testing
import libnacl.utils
import random

nonce = libnacl.utils.rand_nonce()

random.seed(nonce)
otp = random.getrandbits(128)

print(nonce)
print(otp)
print(ord("a"), ord("z"))
print((ord("a")-19) % 26, (ord("z")-19) % 26)

a = 13
b = 60
c = a ^ b

d = 65535
e = 17199
f = d ^ e

print("Testing XOR operation...")
print(a, "XOR", b, "=", c)
print(d, "XOR", e, "=", f)