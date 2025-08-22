import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { SelectionTypesService, SelectionTypesSchema, ProcessSelectionResult } from '../services/selection-types.service';

@Component({
  selector: 'app-test-component1',
  templateUrl: './test-component1.component.html',
  styleUrls: ['./test-component1.component.css']
})
export class TestComponent1Component implements OnInit {
  checkboxForm: FormGroup;
  schema: SelectionTypesSchema | null = null;
  submitResult: ProcessSelectionResult | null = null;
  loading = false;

  constructor(private selectionTypesService: SelectionTypesService) {
    this.checkboxForm = new FormGroup({
      All: new FormControl(false),
      Test1: new FormControl(false),
      Test2: new FormControl(false)
    });
  }

  ngOnInit(): void {
    this.loadSchema();
    this.setupCheckboxLogic();
  }

  loadSchema(): void {
    this.selectionTypesService.getSelectionTypes().subscribe({
      next: (schema) => {
        this.schema = schema;
      },
      error: (error) => {
        console.error('Error loading schema:', error);
      }
    });
  }

  setupCheckboxLogic(): void {
    // Listen to changes on All checkbox
    this.checkboxForm.get('All')?.valueChanges.subscribe(isAllSelected => {
      if (isAllSelected) {
        // If All is selected, select Test1 and Test2
        this.checkboxForm.patchValue({
          Test1: true,
          Test2: true
        }, { emitEvent: false });
      } else {
        // If All is unchecked, uncheck Test1 and Test2
        this.checkboxForm.patchValue({
          Test1: false,
          Test2: false
        }, { emitEvent: false });
      }
    });

    // Listen to changes on Test1 and Test2 checkboxes
    this.checkboxForm.get('Test1')?.valueChanges.subscribe(() => this.updateAllCheckbox());
    this.checkboxForm.get('Test2')?.valueChanges.subscribe(() => this.updateAllCheckbox());
  }

  updateAllCheckbox(): void {
    const test1Selected = this.checkboxForm.get('Test1')?.value;
    const test2Selected = this.checkboxForm.get('Test2')?.value;
    
    if (test1Selected && test2Selected) {
      // If both Test1 and Test2 are selected, select All
      this.checkboxForm.patchValue({ All: true }, { emitEvent: false });
    } else {
      // If either Test1 or Test2 is not selected, uncheck All
      this.checkboxForm.patchValue({ All: false }, { emitEvent: false });
    }
  }

  getSelectedValues(): string[] {
    const selectedValues: string[] = [];
    Object.keys(this.checkboxForm.controls).forEach(key => {
      if (this.checkboxForm.get(key)?.value) {
        selectedValues.push(key);
      }
    });
    return selectedValues;
  }

  onSubmit(): void {
    this.loading = true;
    const selectedValues = this.getSelectedValues();
    
    this.selectionTypesService.processSelectionTypes(selectedValues).subscribe({
      next: (result) => {
        this.submitResult = result;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error submitting selection:', error);
        this.submitResult = {
          isSuccess: false,
          message: 'Error occurred while submitting selection'
        };
        this.loading = false;
      }
    });
  }
}
