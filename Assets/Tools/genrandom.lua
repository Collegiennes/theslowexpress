math.randomseed(os.time())
for i = 1, 256 do
  io.write(math.floor(math.random()*0x100000000), ", ")
end
