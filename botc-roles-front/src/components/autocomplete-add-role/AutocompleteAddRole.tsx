import { Role } from "@/entities/Role";
import { Container, Input, Loading, Spacer } from "@nextui-org/react";
import { Fragment, useState } from "react";
import { removeDiaLowerCase } from "../helper/string";
import Classes from "./AutocompleteAddRole.module.css";
import { X } from "react-feather";
import ListItemRole from "../list-stats/ListItemRole";

export default function AutocompleteAddRole(props: {
  roles: Role[];
  delete: any;
}) {
  const allRoles = props.roles;
  const [isVisible, setIsVisible] = useState(false);
  const [visibleRoles, setVisibleRoles] = useState<Role[]>(
    getAllVisibleRoles("")
  );
  if (props.roles.length === 0) return <Loading />;

  // All roles filtered with the input
  function getAllVisibleRoles(filter: string) {
    return allRoles.filter((role) =>
      removeDiaLowerCase(role.name).includes(removeDiaLowerCase(filter))
    );
  }

  // When the user types
  function inputChanged(inputValue: string) {
    setVisibleRoles(getAllVisibleRoles(inputValue));
  }

  // Hides/Show the list of roles filtered
  function setValuesToSelectVisible(visible: boolean) {
    setIsVisible(visible);
    setVisibleRoles(getAllVisibleRoles(""));
  }

  function onSelectRole(id: number) {
    console.log(id);
  }

  return (
    <div className={Classes["autocomplete-role"]}>
      <div className={Classes["input-container"]}>
        <Input
          css={{ flex: 1 }}
          labelPlaceholder="Rôle"
          aria-label="Rôle"
          clearable
          bordered
          // value={roleSelected}
          onChange={(event) => inputChanged(event.target.value)}
          onFocus={() => setValuesToSelectVisible(true)}
          // onClearClick={() => inputChanged("")}
          // onBlur={() => setValuesToSelectVisible(false)}
        ></Input>
        <Spacer x={0.75} />
        <X className={Classes.delete} onClick={props.delete} />
      </div>
      {isVisible && <Spacer y={0.75} />}
      {isVisible && (
        <div className={Classes["container-roles-values"]}>
          {visibleRoles.map((role) => (
            <div key={role.id} className={Classes["role-item"]}>
              <ListItemRole
                image={role.name}
                characterType={role.characterType}
                onClick={() => onSelectRole(role.id)}
              />
              <Spacer y={0.75} />
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
