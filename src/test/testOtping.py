# This is still in the works

import os, sys
from os import path

sys.path.append(path.dirname(path.dirname(path.abspath(__file__))))

from otping import generateOTP
from otping import unbytify
from otping import longOTP
import libnacl
import libnacl.utils
import random


strVar = "quick brown fox"

def testGenerateOtp():

	random.seed(0)
	key = str(1234)
	bitVar = random.getrandbits(128)

	otpFaulty = generateOTP()
	otpNoKey = generateOTP(strVar)
	otpKey = generateOTP(strVar, key)
	otpBits = generateOTP(bitVar)

	assert otpFaulty == 1
	assert unbytify(otpNoKey) == 7186761680606802985906748832157635480350444277684354552229810452105801312747
	assert unbytify(otpKey) == 67131006667747440599715108554549776574111610289846279238587810320881788223302
	assert unbytify(otpBits) == 98995944724873709172958378693440554281101772036497867124618129284653415819279

	print("generateOTP() working properly")


def testLongOtp():

	longOtpFaulty = longOTP()
	longOtpZero = longOTP(strVar, 0)
	longOtpTwo = longOTP(strVar)
	longOtpThree = longOTP(strVar, 3)

	assert longOtpFaulty == 1
	assert longOtpZero == 2
	assert longOtpTwo == [56813698841288174747865190992797553391219800475463786729908188707207051426913, 45579226694355316895309119577087611133742399265902707413661134215123289648663]
	assert longOtpThree == [56813698841288174747865190992797553391219800475463786729908188707207051426913, 45579226694355316895309119577087611133742399265902707413661134215123289648663, 76417885655670812094827766870684544491666482421571288837733633568929303939824]

	print("longOTP() working properly")

def runTests():
	testGenerateOtp()
	testLongOtp()

runTests()