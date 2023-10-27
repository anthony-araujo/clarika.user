import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { finalize, map } from "rxjs/operators";

import { IState, State } from "../state.model";
import { StateService } from "../service/state.service";
import { ICountry } from "app/entities/country/country.model";
import { CountryService } from "app/entities/country/service/country.service";

@Component({
  selector: "jhi-state-update",
  templateUrl: "./state-update.component.html",
})
export class StateUpdateComponent implements OnInit {
  isSaving = false;

  countriesSharedCollection: ICountry[] = [];

  editForm = this.fb.group({
    id: [],
    name: [null, [Validators.required]],
    country: [],
  });

  constructor(
    protected stateService: StateService,
    protected countryService: CountryService,
    protected activatedRoute: ActivatedRoute,
    protected fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ state }) => {
      this.updateForm(state);

      this.loadRelationshipsOptions();
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const state = this.createFromForm();
    if (state.id !== undefined) {
      this.subscribeToSaveResponse(this.stateService.update(state));
    } else {
      this.subscribeToSaveResponse(this.stateService.create(state));
    }
  }

  trackCountryById(_index: number, item: ICountry): number {
    return item.id!;
  }

  protected subscribeToSaveResponse(
    result: Observable<HttpResponse<IState>>
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

  protected updateForm(state: IState): void {
    this.editForm.patchValue({
      id: state.id,
      name: state.name,
      country: state.country,
    });

    this.countriesSharedCollection =
      this.countryService.addCountryToCollectionIfMissing(
        this.countriesSharedCollection,
        state.country
      );
  }

  protected loadRelationshipsOptions(): void {
    this.countryService
      .query()
      .pipe(map((res: HttpResponse<ICountry[]>) => res.body ?? []))
      .pipe(
        map((countries: ICountry[]) =>
          this.countryService.addCountryToCollectionIfMissing(
            countries,
            this.editForm.get("country")!.value
          )
        )
      )
      .subscribe(
        (countries: ICountry[]) => (this.countriesSharedCollection = countries)
      );
  }

  protected createFromForm(): IState {
    return {
      ...new State(),
      id: this.editForm.get(["id"])!.value,
      name: this.editForm.get(["name"])!.value,
      country: this.editForm.get(["country"])!.value,
    };
  }
}
