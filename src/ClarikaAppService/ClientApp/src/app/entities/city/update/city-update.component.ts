import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { finalize, map } from "rxjs/operators";

import { ICity, City } from "../city.model";
import { CityService } from "../service/city.service";
import { IState } from "app/entities/state/state.model";
import { StateService } from "app/entities/state/service/state.service";

@Component({
  selector: "jhi-city-update",
  templateUrl: "./city-update.component.html",
})
export class CityUpdateComponent implements OnInit {
  isSaving = false;

  statesSharedCollection: IState[] = [];

  editForm = this.fb.group({
    id: [],
    name: [null, [Validators.required]],
    state: [],
  });

  constructor(
    protected cityService: CityService,
    protected stateService: StateService,
    protected activatedRoute: ActivatedRoute,
    protected fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ city }) => {
      this.updateForm(city);

      this.loadRelationshipsOptions();
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const city = this.createFromForm();
    if (city.id !== undefined) {
      this.subscribeToSaveResponse(this.cityService.update(city));
    } else {
      this.subscribeToSaveResponse(this.cityService.create(city));
    }
  }

  trackStateById(_index: number, item: IState): number {
    return item.id!;
  }

  protected subscribeToSaveResponse(
    result: Observable<HttpResponse<ICity>>
  ): void {
    result.pipe(finalize(() => this.onSaveFinalize())).subscribe({
      next: () => this.onSaveSuccess(),
      error: () => this.onSaveError(),
    });
  }

  protected onSaveSuccess(): void {
    this.previousState();
  }

  protected onSaveError(): void {
    // Api for inheritance.
  }

  protected onSaveFinalize(): void {
    this.isSaving = false;
  }

  protected updateForm(city: ICity): void {
    this.editForm.patchValue({
      id: city.id,
      name: city.name,
      state: city.state,
    });

    this.statesSharedCollection =
      this.stateService.addStateToCollectionIfMissing(
        this.statesSharedCollection,
        city.state
      );
  }

  protected loadRelationshipsOptions(): void {
    this.stateService
      .query()
      .pipe(map((res: HttpResponse<IState[]>) => res.body ?? []))
      .pipe(
        map((states: IState[]) =>
          this.stateService.addStateToCollectionIfMissing(
            states,
            this.editForm.get("state")!.value
          )
        )
      )
      .subscribe((states: IState[]) => (this.statesSharedCollection = states));
  }

  protected createFromForm(): ICity {
    return {
      ...new City(),
      id: this.editForm.get(["id"])!.value,
      name: this.editForm.get(["name"])!.value,
      state: this.editForm.get(["state"])!.value,
    };
  }
}
