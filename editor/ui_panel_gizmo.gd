extends EditorNode3DGizmoPlugin


func _init():
	create_material("main", Color(1,0,0))


func _get_gizmo_name():
	return "UIPanel"
	
func _has_gizmo(node):
	return node is UIPanel

func _redraw(gizmo):
	gizmo.clear()
	
	var panel = gizmo.get_node_3d() as UIPanel
	var lines = PackedVector3Array()
	
	var x = panel.PanelSize.x;
	var y = panel.PanelSize.y;
	
	#diagonal
	lines.push_back(Vector3(-0.5 * x, -0.5 * y, 0))
	lines.push_back(Vector3( 0.5 * x,  0.5 * y, 0))
	
	#rectangle
	lines.push_back(Vector3(-0.5 * x, -0.5 * y, 0))
	lines.push_back(Vector3(-0.5 * x,  0.5 * y, 0))
	lines.push_back(Vector3(-0.5 * x,  0.5 * y, 0))
	lines.push_back(Vector3( 0.5 * x,  0.5 * y, 0))
	lines.push_back(Vector3( 0.5 * x,  0.5 * y, 0))
	lines.push_back(Vector3( 0.5 * x, -0.5 * y, 0))
	lines.push_back(Vector3( 0.5 * x, -0.5 * y, 0))
	lines.push_back(Vector3(-0.5 * x, -0.5 * y, 0))
	
	gizmo.add_lines(lines, get_material("main", gizmo), false)
