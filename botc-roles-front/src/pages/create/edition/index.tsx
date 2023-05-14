import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer } from "@nextui-org/react";
import { getAllEditions, getAllRoles } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, PlusCircle, XOctagon } from "react-feather";
import AutocompleteAddRole from "@/components/autocomplete-add-role/AutocompleteAddRole";
import { Role, getNewEmptyRole } from "@/entities/Role";
import { removeDiaLowerCase } from "@/components/helper/string";

export default function CreateEdition() {
  const [editionName, setEditionName] = useState("");
  const [message, setMessage] = useState(<Fragment />);
  const [roles, setRoles] = useState<Role[]>([]);
  const [dropDownRoles, setDropDownRoles] = useState<Role[]>([]);

  const [editions, setEditions] = useState<string[]>([]);
  useEffect(() => {
    async function initEditions() {
      const tempEditions = (await getAllEditions()).map((edition) => {
        return edition.name;
      });
      setEditions(tempEditions);
      const tempRoles = (await getAllRoles()).map((role) => {
        return role;
      });
      setRoles(tempRoles);
    }
    initEditions();
  }, []);

  const title = <Title>Création d{"'"}un nouveau module</Title>;

  async function createEdition() {
    console.log(dropDownRoles);
    return;

    // if (characterType === undefined || alignment === undefined) return;

    // if (await createNewEdition(editionName, characterType, alignment)) {
    //   editions.push(editionName);
    //   setEditionName("");
    //   setEditions(editions);
    //   setResetCharacterType(resetCharacterType + " ");
    //   setResetAlignment(resetAlignment + " ");

    //   updateMessage(false, `Module "${editionName}" enregistré correctement.`);
    // } else {
    //   //Erreur
    //   updateMessage(
    //     true,
    //     "Une erreur est survenue lors de l'enregistrement du module."
    //   );
    // }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  function canCreateEdition() {
    // Can create a edition when
    //  - all params are set
    //  - the name is unique
    return (
      editionName !== "" &&
      editions.filter(
        (p) => removeDiaLowerCase(p) === removeDiaLowerCase(editionName)
      ).length === 0
      // && characterType !== undefined &&
      // alignment !== undefined
    );
  }

  function editionNameChanged(value: string) {
    setEditionName(value);
    checkError(value);
  }

  function checkError(editionName: string) {
    setMessage(<Fragment />);

    if (editionName === "") {
      updateMessage(true, "Le nom du module ne doit pas être vide.");
      return;
    }

    if (editions.filter((p) => p === editionName).length !== 0) {
      updateMessage(true, `Le module '${editionName}' existe déjà.`);
      return;
    }
  }

  function addRole() {
    setDropDownRoles([getNewEmptyRole(), ...dropDownRoles]);
  }
  function removeRole(index: number) {
    setDropDownRoles(dropDownRoles.filter((r, i) => i !== index));
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          clearable
          bordered
          labelPlaceholder="Nom"
          aria-label="Nom"
          value={editionName}
          onChange={(event) => editionNameChanged(event.target.value)}
        ></Input>
        <Spacer y={3} />
        {dropDownRoles.map((dropDownRole, index) => (
          <Fragment key={index}>
            <AutocompleteAddRole
              roles={roles}
              onDelete={() => removeRole(index)}
              onSelectRole={(role: Role) => (dropDownRoles[index] = role)}
            />
            <Spacer y={1.5} />
          </Fragment>
        ))}
        <Spacer y={1.5} />
        <Button
          auto
          color="success"
          icon={<PlusCircle />}
          onPress={addRole}
        ></Button>
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        onPress={createEdition}
        disabled={!canCreateEdition()}
      >
        Créer un module
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
