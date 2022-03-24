def create_plant(name,materialFilePath)
  model = Sketchup.active_model
  definitions = model.definitions
  
  
  if definitions[name] != nil
    #TODO fix so that duplicates are not created [DONE]
    #TODO fix so that material is updated here
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
  material = materials.load(materialFilePath)
  face.material = material
  component = group.to_component
  component.definition.name = name
  component.definition.behavior.always_face_camera=true
  model.commit_operation
end

create_plant("rose 06", "G:\\_githubClones\\SketchupMaterialGenerator\\bin\\Debug\\testFolder\\rose 06.skm")
#create_plant("plant2")
