

def add_update_material(fileDir, name, fileExt)
  model = Sketchup.active_model
  materials = model.materials
  material = materials[name]
  if material == nil
    material = materials.add(name)
  end
  material.texture = fileDir + "\\" + name + fileExt
  #material.save_as(fileDir + "\\" + name + ".skm")
end

def create_update_plant(fileDir, name, fileExt, yPos)
  model = Sketchup.active_model
  definitions = model.definitions
  
  add_update_material(fileDir, name, fileExt)

  if definitions[name] != nil
    #TODO fix so that duplicates are not created [DONE]
    #TODO fix so that material is updated here [DONE] updated above
    #definitions[name].save_as(fileDir + "\\" + name + ".skp")
    return
  end
  group = model.active_entities.add_group
  entities = group.entities
  centerpoint = Geom::Point3d.new
  centerpoint.z = 10.mm
  
  vector = Geom::Vector3d.new(0,0,1)
  edgearray = entities.add_circle(centerpoint, vector, 100.mm)
  face = entities.add_face(edgearray) 
  face.pushpull(-10.mm)
  materials = Sketchup.active_model.materials
  #material = materials.load(materialFilePath)
  face.material = name
  mapping = [
    Geom::Point3d.new(-100.mm,-100.mm,0), # Model coordinate
    Geom::Point3d.new(0,0,0), # UV coordinate
  ]
  on_front = true
  face.position_material(model.materials[name], mapping, on_front)
  component = group.to_component
  component.definition.name = name
  component.definition.behavior.always_face_camera=true
  
  component.definition.save_as(fileDir + "\\" + name + ".skp")
  
  new_pos = Geom::Point3d.new
  new_pos.y = yPos
  component.transform! Geom::Transformation.new(
    component.transformation.origin.vector_to(new_pos)
  )
  #component.definition.save_as(fileDir + "\\" + name + ".skp")
  model.commit_operation
end

def create_update_plants_in_dir(dir)
  curr_y = 0
  Dir.glob(dir + "/*.jpg") do |file|
    bName = File.basename(file, ".*")
    fExt = File.extname(file)
    #puts "Create/Update Plant: #{bName} #{fExt}"
    create_update_plant(dir, bName, fExt, curr_y)
    curr_y = curr_y + 200.mm
  end
  Dir.glob(dir + "/*.png") do |file|
    bName = File.basename(file, ".*")
    fExt = File.extname(file)
    #puts "Create/Update Plant: #{bName} #{fExt}"
    create_update_plant(dir, bName, fExt, curr_y)
    curr_y = curr_y + 200.mm
  end
end

def exec()
  dir = UI.select_directory(title: "Select Image Directory", directory: Sketchup.active_model.path)
  if dir != nil
    create_update_plants_in_dir(dir)
  end
end

def create_toolbar()
  toolbar = UI::Toolbar.new("Importera Bilder till växter")
  cmd = UI::Command.new("Test") {
    exec()
  }
  cmd.small_icon = "ToolPencilSmall.png"
  cmd.large_icon = "ToolPencilLarge.png"
  cmd.tooltip = "Importera Bilder till växter"
  cmd.status_bar_text = ""
  cmd.menu_text = "Importera Bilder till växter"
  toolbar = toolbar.add_item(cmd)
  toolbar.show
end

exec()





#create_update_plant("C:\\scetchupträd\\rosor","Alchymist",".jpg")
#create_update_plant("C:\\scetchupträd\\rosor","Bienewelder röd",".jpg")
#create_update_plant("C:\\scetchupträd\\rosor","Amber queen",".jpg")
