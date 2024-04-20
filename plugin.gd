@tool
extends EditorPlugin


const UIPanelGizmoPlugin = preload("res://addons/Godot-VR-UI/editor/ui_panel_gizmo.gd")
var ui_panel_gizmo_plugin = UIPanelGizmoPlugin.new()


func _enter_tree():
	add_node_3d_gizmo_plugin(ui_panel_gizmo_plugin)


func _exit_tree():
	remove_node_3d_gizmo_plugin(ui_panel_gizmo_plugin)
