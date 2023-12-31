class_name WFC2DGenerator

extends Node
## Generates content of a map (TileMap or GridMap) using WFC algorithm

@export_node_path("TileMap", "GridMap")
var target: NodePath
############################################################### start
# Optionally set target from script instead of through inspector
var target_node: Node
############################################################### end

@export
var rect: Rect2i

@export
@export_category("Rules")
var rules: WFCRules2D = WFCRules2D.new()

@export_node_path("TileMap", "GridMap")
var positive_sample: NodePath

@export_node_path("TileMap", "GridMap")
var negative_sample: NodePath

@export
var solver_settings: WFCSolverSettings = WFCSolverSettings.new()

############################################################### start
@export var positive_tile_map_set: Array[TileMap]
@export var negative_tile_map_set: Array[TileMap]

@export_category("Extras")
@export var tile_constraint_mapper: TileConstraintMapper = null
@export var prohibited_tile_collection: ProhibitedTileCollection = null
@export var tile_subset_whitelist: TileSubsetWhitelist = null
@export var interchangeable_tiles_rules_sharer: InterchangeableTileRulesSharer = null
############################################################### end

@export
@export_category("Runner")
var multithreaded_runner_settings: WFCMultithreadedRunnerSettings = WFCMultithreadedRunnerSettings.new()

@export
var main_thread_runner_settings: WFCMainThreadRunnerSettings = WFCMainThreadRunnerSettings.new()

@export
var use_multithreading: bool = true

@export
@export_category("Behavior")
var start_on_ready: bool = false

@export
var render_intermediate_results: bool = false

@export
@export_category("Debug mode")
var print_rules: bool = false

signal done(generation_error: bool)


var _runner: WFCSolverRunner = null

func _create_runner() -> WFCSolverRunner:
	if use_multithreading:
		var res: WFCMultithreadedSolverRunner = WFCMultithreadedSolverRunner.new()

		if multithreaded_runner_settings != null:
			res.runner_settings = multithreaded_runner_settings

		res.solver_settings = solver_settings
		return res
	else:
		var res: WFCMainThreadSolverRunner = WFCMainThreadSolverRunner.new()

		if main_thread_runner_settings != null:
			res.runner_settings = main_thread_runner_settings

		res.solver_settings = solver_settings
		return res


func _create_mapper(map: Node) -> WFCMapper2D:
	match map.get_class():
		"TileMap":
			return WFCTileMapMapper2D.new()
		"GridMap":
			return WFCGridMapMapper2D.new()
		var cname:
			push_error("Unsupported map type for WFC2DGenerator: " + cname)
			@warning_ignore("assert_always_false")
			assert(false)
			return null

func _create_problem(settings: WFC2DProblem.WFC2DProblemSettings, map: Node) -> WFC2DProblem:
	return WFC2DProblem.new(settings, map)

func _exit_tree():
	if _runner != null:
		_runner.interrupt()
		_runner = null

func start():
############################################################### start
	assert(_runner == null)
	assert(target != null || target_node != null)
	assert(rect.has_area())
	
	# Get node from path if target_node is not already assigned
	if target_node == null:
		target_node = get_node(target)
	assert(target_node != null)
############################################################### end

	if not rules.is_ready():
		assert(positive_sample != null)
		
		var positive_sample_node: Node = get_node(positive_sample)
		assert(positive_sample_node != null)
		
		if rules == null:
			rules = WFCRules2D.new()
		else:
			rules = rules.duplicate(false) as WFCRules2D

			assert(rules != null)
		
		if rules.mapper == null:
			rules.mapper = _create_mapper(target_node)

		if not rules.mapper.is_ready():
			rules.mapper.learn_from(positive_sample_node)
############################################################### start
			# Add TileSets from additional positive TileMaps
			for tile_map in positive_tile_map_set:
				rules.mapper.learn_from(tile_map)
############################################################### end
		
		rules.learn_from(positive_sample_node)
		
############################################################### start
		# Learn from additional positive TileMaps
		for tile_map in positive_tile_map_set:
			rules.learn_from(tile_map)
		
		# Share rules between interchangeable tiles
		if interchangeable_tiles_rules_sharer != null:
			interchangeable_tiles_rules_sharer.wfc_rules = rules
			interchangeable_tiles_rules_sharer.share_interchangeable_tile_rules(true)
############################################################### end
		
		# removed "rules.complete_matrices" condition
		if negative_sample != null and not negative_sample.is_empty():
			var negative_sample_node: Node = get_node(negative_sample)
			
			if negative_sample_node != null:
				rules.learn_negative_from(negative_sample_node)

############################################################### start
		# Learn from additional negative TileMaps
		for tile_map in negative_tile_map_set:
			rules.learn_negative_from(tile_map)
		
		# Share rules between interchangeable tiles
		if interchangeable_tiles_rules_sharer != null:
			interchangeable_tiles_rules_sharer.share_interchangeable_tile_rules(false)
############################################################### end

		if print_rules and OS.is_debug_build():
			print_debug('Rules learned:\n', rules.format())
			
			print_debug('Influence range: ', rules.get_influence_range())

	var problem_settings: WFC2DProblem.WFC2DProblemSettings = WFC2DProblem.WFC2DProblemSettings.new()
	
	problem_settings.rules = rules
	problem_settings.rect = rect

	var problem: WFC2DProblem = _create_problem(problem_settings, target_node)
	
############################################################### start
	# Add constraint mapper if provided
	problem.tile_constraint_mapper = tile_constraint_mapper
	if tile_constraint_mapper != null:
		tile_constraint_mapper.problem = problem
	# Add prohibited tiles if defined
	problem.prohibited_tile_collection = prohibited_tile_collection
	if prohibited_tile_collection != null:
		prohibited_tile_collection.problem = problem
	problem.tile_subset_whitelist = tile_subset_whitelist
	if tile_subset_whitelist != null:
		tile_subset_whitelist.problem = problem
############################################################### end

	_runner = _create_runner()
	
	_runner.start(problem)

############################################################### start
	_runner.all_solved.connect(_on_all_solved)
############################################################### end
	_runner.sub_problem_solved.connect(_on_solved)
	_runner.partial_solution.connect(_on_partial_solution)

func _on_solved(problem: WFC2DProblem, state: WFCSolverState):
	if state != null:
		problem.render_state_to_map(state)

func _on_partial_solution(problem: WFC2DProblem, state: WFCSolverState):
	if not render_intermediate_results:
		return
	
	_on_solved(problem, state)

############################################################### start
func _on_all_solved():
	done.emit(_runner.has_generation_error)
############################################################### end

func _ready():
	if start_on_ready:
		start()

func _process(_delta):
	if _runner != null and _runner.is_running():
		_runner.update()







