def add_update_material(fileDir, name, fileExt)
  model = Sketchup.active_model
  materials = model.materials
  material = materials[name]
  if material == nil
    material = materials.add(name)
  end
  material.texture = fileDir + "\\" + name + fileExt
  material.save_as(fileDir + "\\" + name + ".skm")
end

add_update_material("C:\\scetchupträd\\rosor","Alchymist",".jpg")
add_update_material("C:\\scetchupträd\\rosor","Bienewelder röd",".jpg")
add_update_material("C:\\scetchupträd\\rosor","Amber queen",".jpg")
